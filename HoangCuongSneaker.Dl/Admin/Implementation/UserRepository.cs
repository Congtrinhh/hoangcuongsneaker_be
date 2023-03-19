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

        public override async Task<UserDto> Get(int id)
        {
            using (var conn = new MySqlConnection(_connectionString))
            {
                conn.Open();

                var sql = "select * from user where id=@id";

                var user = await conn.QueryFirstOrDefaultAsync<User>(sql: sql, commandType: System.Data.CommandType.Text, param: new { @id = id });
                if (user is not null && user.Id > 0)
                {
                    var modelResult = _mapper.Map<UserDto>(user);
                    return modelResult;
                }
                else
                {
                    return new UserDto();
                }
            }
        }

        public override async Task<int> Delete(int id)
        {
            using (var conn = new MySqlConnection(_connectionString))
            {
                conn.Open();

                var sql = "update user set is_active=false where id=@id";

                int affectedRows = await conn.ExecuteAsync(sql: sql, commandType: System.Data.CommandType.Text, param: new { @id = id });
                return affectedRows;
            }
        }

        public override async Task<UserDto> Create(UserDto model)
        {
            using (var conn = new MySqlConnection(_connectionString))
            {
                conn.Open();

                var procUserInsert = "proc_user_insert";

                var user = _mapper.Map<User>(model);

                var insertedUserId = await conn.ExecuteScalarAsync<int>(sql: procUserInsert, commandType: System.Data.CommandType.StoredProcedure, param: user);
                if (insertedUserId > 0)
                {
                    var insertedUser = Get(insertedUserId);
                    var modelResult = _mapper.Map<UserDto>(insertedUser);
                    return modelResult;
                }
                return new UserDto();
            }
        }

        public override async Task<UserDto> Update(UserDto model)
        {
            using (var conn = new MySqlConnection(_connectionString))
            {
                conn.Open();

                var procUserUpdate = "proc_user_update";

                var user = _mapper.Map<User>(model);

                var updatedUserId = await conn.ExecuteScalarAsync<int>(sql: procUserUpdate, commandType: System.Data.CommandType.StoredProcedure, param: user);
                if (updatedUserId> 0)
                {
                    var updatedUser = await Get(updatedUserId);
                    var modelResult = _mapper.Map<UserDto>(updatedUser);
                    return modelResult;
                }
                // throw exception or error response instead
                return new UserDto();
            }
        }

        public override async Task<PagingResponse<UserDto>> GetPaging(PagingRequest pagingRequest)
        {
            using (var conn = new MySqlConnection(_connectionString))
            {
                conn.Open(); 
                
                // build sql
                var sql = BuildSelectStatement(pagingRequest);

                var users = await conn.QueryAsync<User>(sql: sql, commandType: System.Data.CommandType.Text);

                var items = users.ToList().Select(user => _mapper.Map<UserDto>(user)).ToList();
                var pagingResponse = new PagingResponse<UserDto>();
                pagingResponse.PageSize = pagingRequest.PageSize;
                pagingResponse.PageIndex = pagingRequest.PageIndex;
                pagingResponse.Items = items;

                return pagingResponse;
            }
        }

        protected override string BuildSelectStatement(PagingRequest pagingRequest)
        {

            var sql = "select * from user where 1=1";

            if (pagingRequest is UserPagingRequest userPagingRequest)
            {
                if (!string.IsNullOrEmpty(userPagingRequest.UserName))
                {
                    sql += $" and user_name like '%{userPagingRequest.UserName}%'";
                }
                else if (!string.IsNullOrEmpty(userPagingRequest.Email))
                {
                    sql += $" and email like '%{userPagingRequest.Email}%'";
                }
                else if (!string.IsNullOrEmpty(userPagingRequest.PhoneNumber))
                {
                    sql += $" and phone like '%{userPagingRequest.PhoneNumber}%'";
                }


                sql += " order by ";
                userPagingRequest.Sorts.ForEach(sort =>
                {
                    sql += $" {sort.Field} {sort.SortDirection},";
                });
                sql.Remove(sql.Length - 1); // bo dau phay cuoi
            }



            int limit = pagingRequest.PageSize;
            int offset = pagingRequest.PageSize * pagingRequest.PageIndex;
            sql += $" limit {limit} offset {offset}";

            return sql;
        }


    }
}
