using Dapper;
using HoangCuongSneaker.Core;
using HoangCuongSneaker.Core.Dto.Paging;
using HoangCuongSneaker.Core.Dto.Paging.Admin;
using HoangCuongSneaker.Core.Model;
using HoangCuongSneaker.Core.Model.Admin.Order;
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
    public class UserRepository : BaseRepository<UserDto>, IUserRepository
    {
        public UserRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public override async Task<UserDto?> Get(int id, MySqlConnection connection = null)
        {
            connection = connection ?? GetSqlConnection();
            connection.Open();

            var sql = "select * from user where id=@id";

            var user = await connection.QueryFirstOrDefaultAsync<User>(sql: sql, commandType: System.Data.CommandType.Text, param: new { @id = id });
            if (user is not null && user.Id > 0)
            {
                var sqlOrderSelect = "select * from purchase_order where user_id=@userId"; // TODO: dung ham paging tu orderRepository de cai tien hieu nang

                var queriedOrders = (await connection.QueryAsync<Order>(sql: sqlOrderSelect, commandType: System.Data.CommandType.Text, param: new { @userId = user.Id })).ToList();
                var orderDtos = queriedOrders.Select((order) => _mapper.Map<OrderDto>(order)).ToList();

                var modelResult = _mapper.Map<UserDto>(user);
                modelResult.Orders = orderDtos;

                return modelResult;
            }
            return null;
        }

        public override async Task<int> Delete(int id, MySqlConnection connection = null)
        {
            using (var conn = new MySqlConnection(_connectionString))
            {
                conn.Open();

                var sql = "update user set is_active=false where id=@id";

                int affectedRows = await conn.ExecuteAsync(sql: sql, commandType: System.Data.CommandType.Text, param: new { @id = id });
                return affectedRows;
            }
        }

        public override async Task<UserDto?> Create(UserDto model, MySqlConnection connection = null, MySqlTransaction transaction = null)
        {
            using (var conn = new MySqlConnection(_connectionString))
            {
                conn.Open();

                var userFromDb = await GetByUserName(model.UserName);
                if (userFromDb is not null) return null;

                var procUserInsert = "proc_user_insert";

                var user = _mapper.Map<User>(model);
                string passwordHash = BCrypt.Net.BCrypt.HashPassword(model.Password);
                user.Password = passwordHash;

                var insertedUserId = await conn.ExecuteScalarAsync<int>(sql: procUserInsert, commandType: System.Data.CommandType.StoredProcedure, param: user);
                if (insertedUserId > 0)
                {
                    model.Id = insertedUserId;
                    var modelResult = _mapper.Map<UserDto>(model);
                    modelResult.Password = string.Empty;// không trả về thông tin mật khẩu
                    return modelResult;
                }
                return null;
            }
        }

        public override async Task<UserDto?> Update(UserDto model, MySqlConnection connection = null, MySqlTransaction transaction = null)
        {
            using (var conn = new MySqlConnection(_connectionString))
            {
                conn.Open();

                var procUserUpdate = "proc_user_update";

                var user = _mapper.Map<User>(model);

                var affectedRows = await conn.ExecuteAsync(sql: procUserUpdate, commandType: System.Data.CommandType.StoredProcedure, param: user);
                if (affectedRows > 0)
                {
                    var updatedUser = await Get(user.Id);
                    var modelResult = _mapper.Map<UserDto>(updatedUser);
                    return modelResult;
                }
                return null;
            }
        }

        public async Task<PagingResponse<UserDto>> GetPaging(PagingRequest pagingRequest, MySqlConnection connection = null)
        {
            connection = connection ?? GetSqlConnection();
            connection.Open();

            var sqlSelect = GetSqlGetPaging(pagingRequest);
            var sqlTotalCount = GetSqlGetTotalCountPaging(pagingRequest);

            var queriedUsers = (await connection.QueryAsync<User>(sql: sqlSelect, commandType: System.Data.CommandType.Text)).ToList();
            int totalCount = await connection.QueryFirstOrDefaultAsync<int>(sql: sqlTotalCount, commandType: System.Data.CommandType.Text);

            var sqlOrderSelect = "select * from purchase_order where user_id=@userId"; // TODO: dung ham paging tu orderRepository de cai tien hieu nang
            var userDtos = new List<UserDto>();
            foreach (var user in queriedUsers)
            {
                var userDto = _mapper.Map<UserDto>(user);

                var queriedOrders = (await connection.QueryAsync<Order>(sql: sqlOrderSelect, commandType: System.Data.CommandType.Text, param: new { @userId = user.Id })).ToList();
                var orderDtos = queriedOrders.Select((order) => _mapper.Map<OrderDto>(order)).ToList();

                userDto.Orders = orderDtos;
                userDtos.Add(userDto);
            }

            var response = new PagingResponse<UserDto>();
            response.Items = userDtos;
            response.TotalRecord = totalCount;

            return response;
        }

        public override string GetSqlGetPaging(PagingRequest pagingRequest)
        {
            var sql = "select * from user where 1=1";

            if (pagingRequest is UserPagingRequest p)
            {
                if (!string.IsNullOrEmpty(p.FilterValue))
                {
                    sql += " and (";
                    sql += $" user_name like '%{p.FilterValue}%' ";
                    sql += $" or email like '%{p.FilterValue}%' ";
                    sql += $" or phone like '%{p.FilterValue}%' ";
                    sql += " ) ";

                }
            }

            int limit = pagingRequest.PageSize;
            int offset = pagingRequest.PageSize * pagingRequest.PageIndex;
            sql += " order by updated_at desc, created_at desc ";
            sql += $" limit {limit} offset {offset}";

            return sql;
        }

        public override string GetSqlGetTotalCountPaging(PagingRequest pagingRequest)
        {
            var sql = "select * from user where 1=1";

            if (pagingRequest is UserPagingRequest p)
            {
                if (!string.IsNullOrEmpty(p.FilterValue))
                {
                    sql += " and (";
                    sql += $" user_name like '%{p.FilterValue}%' ";
                    sql += $" or email like '%{p.FilterValue}%' ";
                    sql += $" or phone like '%{p.FilterValue}%' ";
                    sql += " ) ";

                }
            }

            return sql;
        }
 
        public async Task<UserDto?> GetByUserName(string userName, MySqlConnection connection = null)
        {
            connection = connection ?? GetSqlConnection();
            connection.Open();

            var sql = "select * from user where user_name=@userName";

            var user = await connection.QueryFirstOrDefaultAsync<User>(sql: sql, commandType: System.Data.CommandType.Text, param: new { @userName = userName });
            if (user is not null && user.Id > 0)
            {
                var modelResult = _mapper.Map<UserDto>(user);
                return modelResult;
            }
            return null;
        }
    }
}
