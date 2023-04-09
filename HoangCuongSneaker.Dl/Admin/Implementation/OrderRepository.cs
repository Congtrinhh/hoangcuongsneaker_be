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
            _tableName = "purchase_order";
        }

        public async override Task<OrderDto?> Get(int id, MySqlConnection connection = null)
        {
            connection = connection ?? GetSqlConnection();
            await connection.OpenAsync();// error prone

            var sql = "select * from purchase_order where id=@id";
            var order = await connection.QueryFirstOrDefaultAsync<Order>(sql: sql, commandType: System.Data.CommandType.Text, param: new { @id = id });

            if (order != null && order.Id > 0)
            {
                var sqlUserSelect = "select * from user where id = @id";

                var userId = order.UserId;
                var user = await connection.QueryFirstOrDefaultAsync<User>(sqlUserSelect, commandType: System.Data.CommandType.Text, param: new { @id = userId });

                if (user is not null && user.Id > 0)
                {
                    var sqlOrderItemsSelect = "select * from order_item where order_id = @orderId";
                    var orderItems = (await connection.QueryAsync<OrderItemDto>(sql: sqlOrderItemsSelect, commandType: System.Data.CommandType.Text, param: new { @orderId = order.Id })).ToList();

                    var orderResult = _mapper.Map<Order, OrderDto>(order);
                    orderResult.User = _mapper.Map<User, UserDto>(user);
                    orderResult.OrderItems = orderItems;
                    return orderResult;
                }
            }
            return null;
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
        public Task<PagingResponse<OrderDto>> GetPaging(PagingRequest pagingRequest, MySqlConnection connection = null)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// update order and order item
        /// not need to update user because user model has to update its own data
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async override Task<OrderDto?> Update(OrderDto model, MySqlConnection connection = null)
        {
            var productFromDb = Get(model.Id);
            if (productFromDb != null)
            {
                connection = connection ?? GetSqlConnection();
                await connection.OpenAsync();// error prone

                var sql = "proc_product_update";

                var updatedProduct = await connection.ExecuteScalarAsync<Product>(sql: sql, commandType: System.Data.CommandType.StoredProcedure, param: model);
                if (updatedProduct is not null)
                {
                    // get relevant data
                    var updatedProductDto = await Get(updatedProduct.Id);
                    return updatedProductDto;
                }
                return null;
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
        public override async Task<OrderDto?> Create(OrderDto model, MySqlConnection connection = null)
        {
            connection = connection ?? GetSqlConnection();
            await connection.OpenAsync();// error prone
            var transaction = connection.BeginTransaction();

            var user = await _userRepository.GetByUserName(model.User.UserName);
            if (user is not null && user.Id > 0)
            {
                // insert order
                var procOrderInsert = "proc_purchase_order_insert";
                var orderParam = _mapper.Map<OrderDto, Order>(model); // need to convert because procedure need to receive needed model 
                orderParam.IsActive = true;
                orderParam.ShippedAt = DateTime.MinValue;

                var insertedOrderId = await connection.ExecuteScalarAsync<int>(sql: procOrderInsert, commandType: System.Data.CommandType.StoredProcedure, param: orderParam, transaction: transaction);
                if (insertedOrderId > 0)
                {
                    int insertedOrderItemCount = 0;
                    var orderItemsResult = new List<OrderItemDto>();
                    var procOrderItemInsert = "proc_order_item_insert";
                    for (int i = 0; i < model.OrderItems.Count; i++)
                    {
                        var orderItemParam = model.OrderItems[i];

                        orderItemParam.OrderId = insertedOrderId;
                        orderItemParam.ProductInventoryId = orderItemParam.Id; // vì khi xử lý dưới client, trường Id của order item chính là ProductInventoryId

                        var insertedOrderItemId = await connection.ExecuteScalarAsync<int>(sql: procOrderItemInsert, commandType: System.Data.CommandType.StoredProcedure, param: orderItemParam, transaction: transaction);
                        if (insertedOrderItemId > 0)
                        {
                            insertedOrderItemCount++;  
                            orderItemsResult.Add(orderItemParam);
                        }
                    }
                    if (insertedOrderItemCount != model.OrderItems.Count)
                    {
                        transaction.Rollback();
                    }
                    else
                    {
                        transaction.Commit();
                        var newlyInsertedOrder = await Get(insertedOrderId); 
                        return newlyInsertedOrder;
                    }
                }
                else
                {
                    transaction.Rollback(); 
                }
            }
            return null;
        }

        /// <summary>
        /// check permission to delete (integrate later)
        /// set active state to false
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async override Task<int> Delete(int id, MySqlConnection connection = null)
        {
            connection = connection ?? GetSqlConnection();
            await connection.OpenAsync();

            var sql = "update order_item set is_active = false where id=@id";
            var affectedRow = await connection.ExecuteAsync(sql: sql, commandType: System.Data.CommandType.Text, param: new { @id = id });
            return affectedRow;
        }

    }
}
