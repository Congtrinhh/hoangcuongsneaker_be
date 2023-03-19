using Dapper;
using HoangCuongSneaker.Core;
using HoangCuongSneaker.Core.Dto.Paging;
using HoangCuongSneaker.Core.Model;
using HoangCuongSneaker.Core.Model.Admin.Order;
using HoangCuongSneaker.Core.Utility;
using HoangCuongSneaker.Repository.Admin.Interface;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoangCuongSneaker.Repository.Admin.Implementation
{
    public class OrderRepository : BaseRepository<OrderDto>, IOrderRepository
    {
        protected IUserRepository _userRepository;
        public OrderRepository(IConfiguration configuration, IUserRepository userRepository) : base(configuration)
        {
            _userRepository = userRepository;
        }

        public async override Task<OrderDto> Get(int id)
        {
            using (var conn = new MySqlConnection(_connectionString))
            {
                conn.Open();

                var sql = "select * from purchase_order where id=@id";


                var order = await conn.QueryFirstOrDefaultAsync<Order>(sql: sql, commandType: System.Data.CommandType.Text, param: new { @id = id });
                if (order != null && order.Id > 0)
                {
                    var sqlUserSelect = "select * from user where user_id = @userId";

                    var userId = order.UserId;
                    var user = await conn.QueryFirstOrDefaultAsync<User>(sqlUserSelect, commandType: System.Data.CommandType.Text, param: new { @userId = userId });

                    if (user is not null && user.Id > 0)
                    {
                        var sqlOrderItemsSelect = "select * from order_item where order_id = @orderId";
                        var orderItems = (await conn.QueryAsync<OrderItemDto>(sql: sqlOrderItemsSelect, commandType: System.Data.CommandType.Text, param: new { @orderId = order.Id })).ToList();

                        var orderResult = _mapper.Map<Order, OrderDto>(order);
                        orderResult.User = _mapper.Map<User, UserDto>(user);
                        orderResult.OrderItems = orderItems;
                        return orderResult;
                    }
                }
                return new OrderDto();
            }
        }

        /// <summary>
        /// filter by:
        ///     a shipping status
        ///     is active state
        ///     made by a user
        ///     
        /// sort by:
        ///     created date
        ///     total bill price
        /// </summary>
        /// <param name="pagingRequest"></param>
        /// <returns></returns>
        public override Task<PagingResponse<OrderDto>> GetPaging(PagingRequest pagingRequest)
        { 
            return base.GetPaging(pagingRequest);
        }

        /// <summary>
        /// update order and order item
        /// not need to update user because user model has to update its own data
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async override Task<OrderDto> Update(OrderDto model)
        {
            var productFromDb = Get(model.Id);
            if (productFromDb != null)
            {
                using (var conn = new MySqlConnection(_connectionString))
                {
                    conn.Open();

                    var sql = "proc_product_update";

                    var updatedProduct = await conn.ExecuteScalarAsync<Product>(sql: sql, commandType: System.Data.CommandType.StoredProcedure, param: model);
                    if (updatedProduct is not null)
                    {
                        // get relevant data
                        var updatedProductDto = await Get(updatedProduct.Id);
                        return updatedProductDto;
                    }
                    return null;
                }
            }
            return null;
        }

        /// <summary>
        /// create order first:
        ///     check user exist
        ///     create order
        /// create order items
        /// commit transaction
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public override async Task<OrderDto> Create(OrderDto model)
        {
            using (var conn = new MySqlConnection(_connectionString))
            {
                conn.Open();

                var user = await _userRepository.Get(model.User.Id);
                if (user is not null && user.Id > 0)
                {
                    // insert order
                    var procOrderInsert = "proc_order_insert";
                    var orderParam = _mapper.Map<OrderDto, Order>(model); // need to convert because procedure need to receive needed model 
                    var insertedOrder = await conn.ExecuteScalarAsync<Order>(sql: procOrderInsert, commandType: System.Data.CommandType.StoredProcedure, param: orderParam);
                    if (insertedOrder is not null && insertedOrder.Id > 0)
                    {
                        int insertedOrderItemCount = 0;
                        var orderItemsResult = new List<OrderItemDto>();
                        var procOrderItemInsert = "proc_order_item_insert";
                        for (int i = 0; i < model.OrderItems.Count; i++)
                        {
                            var orderItemParam = model.OrderItems[i];
                            var insertedOrderItem = await conn.ExecuteScalarAsync<OrderItem>(sql: procOrderItemInsert, commandType: System.Data.CommandType.StoredProcedure, param: orderItemParam);
                            if (insertedOrderItem is not null && insertedOrderItem.Id > 0)
                            {
                                insertedOrderItemCount++;
                                var insertedOrderItemDto = _mapper.Map<OrderItem, OrderItemDto>(insertedOrderItem);
                                orderItemsResult.Add(insertedOrderItemDto);
                            }
                        }
                        if (insertedOrderItemCount != model.OrderItems.Count)
                        {
                            //rollback
                        }
                        else
                        {
                            var modelResult = _mapper.Map<Order, OrderDto>(insertedOrder);
                            modelResult.OrderItems = orderItemsResult;
                            modelResult.User = user;
                            return modelResult;
                        }
                    }
                    else
                    {
                        //rollback
                    }
                }
                return new OrderDto();
            }
        }

        /// <summary>
        /// check permission to delete (integrate later)
        /// set active state to false
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async override Task<int> Delete(int id)
        {
            using (var conn = new MySqlConnection(_connectionString))
            {
                conn.Open();

                var sql = "update order_item set is_active = false where id=@id";
                var affectedRow = await conn.ExecuteAsync(sql: sql, commandType: System.Data.CommandType.Text, param: new { @id = id });
                return affectedRow;
            }
        }

    }
}
