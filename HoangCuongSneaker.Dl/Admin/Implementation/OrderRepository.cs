using Dapper;
using HoangCuongSneaker.Core;
using HoangCuongSneaker.Core.Dto.Paging;
using HoangCuongSneaker.Core.Dto.Paging.Admin;
using HoangCuongSneaker.Core.Enum;
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
        protected IProductInventoryRepository _productInventoryRepository;
        public OrderRepository(IConfiguration configuration, IUserRepository userRepository, IProductInventoryRepository productInventoryRepository) : base(configuration)
        {
            _userRepository = userRepository;
            _productInventoryRepository = productInventoryRepository;
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
        public async Task<PagingResponse<OrderDto>> GetPaging(PagingRequest pagingRequest, MySqlConnection connection = null)
        {
            connection = connection ?? GetSqlConnection();
            connection.Open();

            var sqlSelect = GetSqlGetPaging(pagingRequest);
            var sqlTotalCount = GetSqlGetTotalCountPaging(pagingRequest);

            var queriedOrders = (await connection.QueryAsync<Order>(sql: sqlSelect, commandType: System.Data.CommandType.Text)).ToList();
            int totalCount = await connection.QueryFirstOrDefaultAsync<int>(sql: sqlTotalCount, commandType: System.Data.CommandType.Text);

            var orderDtos = new List<OrderDto>();
            foreach (var order in queriedOrders)
            {
                var orderDto = _mapper.Map<OrderDto>(order);

                var user = await _userRepository.Get(order.UserId);

                var sqlOrderItemSelect = "select * from order_item where order_id=@orderId";
                var queriedOrderItems = (await connection.QueryAsync<OrderItemDto>(sql: sqlOrderItemSelect, param: new { @orderId = order.Id }, commandType: System.Data.CommandType.Text)).ToList();

                orderDto.OrderItems = queriedOrderItems;
                orderDto.User = user;
                orderDtos.Add(orderDto);
            }

            var response = new PagingResponse<OrderDto>();
            response.Items = orderDtos.ToList();
            response.TotalRecord = totalCount;

            return response;
        }

        public override string GetSqlGetPaging(PagingRequest pagingRequest)
        {
            var sql = $"select * from purchase_order where 1=1";

            if (pagingRequest is OrderPagingRequest p)
            {
                if (p.ShippingStatus.HasValue)
                {
                    sql += $" and shipping_status = {(int)p.ShippingStatus.Value}";
                }
                if (!string.IsNullOrWhiteSpace(p.FilterValue))
                {
                    sql += $" and code like '%{p.FilterValue}%'";
                }
                if (p.DateFilter.HasValue)
                {
                    sql += GetSqlFilterDate(p.DateFilter);
                }
            }

            int limit = pagingRequest.PageSize;
            int offset = pagingRequest.PageSize * pagingRequest.PageIndex;
            sql += " order by updated_at desc, created_at desc ";
            sql += $" limit {limit} offset {offset}";
            return sql;
        }

        /// <summary>
        /// thêm câu sql lọc theo ngày tạo đơn hàng
        /// </summary>
        /// <param name="dateFilter"></param>
        /// <returns></returns>
        private string GetSqlFilterDate(DateFilterOptionEnum? dateFilter = DateFilterOptionEnum.Today)
        {
            DateTime startDate = DateTime.Now.Date, endDate = DateTime.Now.Date;

            DateTime baseDate = DateTime.Today;
            switch (dateFilter)
            {
                case DateFilterOptionEnum.Today:
                    startDate = baseDate.AddDays(-1);
                    endDate = DateTime.Today;
                    break;
                case DateFilterOptionEnum.ThisWeek:
                    startDate = baseDate.AddDays(-(int)baseDate.DayOfWeek);
                    endDate = startDate.AddDays(7).AddSeconds(-1);
                    break;
                case DateFilterOptionEnum.ThisMonth:
                    startDate = baseDate.AddDays(1 - baseDate.Day);
                    endDate = startDate.AddMonths(1).AddSeconds(-1);
                    break;
                case DateFilterOptionEnum.All:
                default:
                    return string.Empty;
            }
            var sql = $" and created_at between '{startDate.ToString("yyyy-MM-dd h:mm:ss tt")}' and '{endDate.ToString("yyyy-MM-dd h:mm:ss tt")}' ";
            return sql;
        }

        public override string GetSqlGetTotalCountPaging(PagingRequest pagingRequest)
        {
            var sql = "select count(1) from purchase_order where 1=1";

            if (pagingRequest is OrderPagingRequest p)
            {
                if (p.ShippingStatus.HasValue)
                {
                    sql += $" and shipping_status = {(int)p.ShippingStatus.Value}";
                }
                if (!string.IsNullOrWhiteSpace(p.FilterValue))
                {
                    sql += $" and code like '%{p.FilterValue}%'";
                }
                if (p.DateFilter.HasValue)
                {
                    sql += GetSqlFilterDate(p.DateFilter);
                }
            }
            return sql;
        }

        /// <summary>
        /// update order and order item
        /// not need to update user because user model has to update its own data
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async override Task<OrderDto?> Update(OrderDto model, MySqlConnection connection = null, MySqlTransaction transaction = null)
        {
            connection = connection ?? GetSqlConnection();
            connection.Open();// error prone

            var sql = "proc_purchase_order_update";

            var modelUpdate = _mapper.Map<Order>(model);
            int updatedId = await connection.ExecuteScalarAsync<int>(sql: sql, commandType: System.Data.CommandType.StoredProcedure, param: modelUpdate);
            if (updatedId > 0)
            {
                // TODO: update order items
                // update bill price of order
                return model;
            }
            return null;
        }

        public override void BeforeInsert(OrderDto model)
        {

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
        public override async Task<OrderDto?> Create(OrderDto model, MySqlConnection connection = null, MySqlTransaction transaction = null)
        {
            connection = connection ?? GetSqlConnection();
            await connection.OpenAsync();// error prone
            transaction = transaction ?? connection.BeginTransaction();

            var user = await _userRepository.GetByUserName(model.User.UserName);
            if (user is not null && user.Id > 0)
            {
                // insert order
                var procOrderInsert = "proc_purchase_order_insert";


                var orderParam = _mapper.Map<OrderDto, Order>(model); // need to convert because procedure need to receive needed model 
                orderParam.IsActive = true;
                orderParam.ShippedAt = DateTime.MinValue;
                decimal billPrice = 0;
                foreach (var item in model.OrderItems)
                {
                    billPrice += item.Quantity * item.Price;
                }
                orderParam.BillPrice = billPrice;

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
                        // update quantity of each product inventory
                        int productInventoryUpdateCount = 0;
                        foreach (var item in orderItemsResult)
                        {
                            var productInventory = await _productInventoryRepository.Get(item.ProductInventoryId);
                            if (productInventory is not null) productInventory.Quantity -= item.Quantity;
                            else throw new Exception("Không tìm thấy sản phẩm");
                            var productInventoryUpdate = _mapper.Map<ProductInventory>(productInventory);
                            var updatedProductInventory = await _productInventoryRepository.Update(productInventoryUpdate, connection, transaction);
                            if (updatedProductInventory is not null) productInventoryUpdateCount++;
                        }

                        if (productInventoryUpdateCount == orderItemsResult.Count)
                        {

                            transaction.Commit();
                            var newlyInsertedOrder = await Get(insertedOrderId);
                            return newlyInsertedOrder;
                        }
                        return null;
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
