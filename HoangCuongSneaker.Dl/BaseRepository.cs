using Dapper;
using HoangCuongSneaker.Core;
using HoangCuongSneaker.Core.Dto.Paging;
using HoangCuongSneaker.Core.Model;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoangCuongSneaker.Repository
{
    public class BaseRepository<T> : IBaseRepository<T> where T : BaseModel
    {
        #region Property
        protected readonly IConfiguration _configuration;
        protected string _tableName = string.Empty;
        protected string _connectionString = string.Empty;
        #endregion
        public BaseRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _tableName = typeof(T).Name;
            _connectionString = _configuration["ConnectionStrings:DefaultConnection"] ?? string.Empty;
        }

        virtual public async Task<T> Create(T model)
        {
            throw new NotImplementedException();
        }

        virtual public async Task<int> Delete(int id)
        {
            throw new NotImplementedException();
        }

        virtual public async Task<List<T>> GetAll()
        {
            throw new NotImplementedException();
        }

        virtual public async Task<T> Get(int id)
        {
            
            using (var conn = new MySqlConnection(_connectionString))
            {
                string sql = $"select * from {_tableName} where id=@id";
                var paramDictionary = new DynamicParameters();
                paramDictionary.Add("@id", id);
                var res = await conn.QueryFirstOrDefaultAsync<T>(sql: sql, param: paramDictionary, commandType: System.Data.CommandType.Text);
                return res;
            }
        }

        virtual public async Task<PagingResponse<T>> GetPaging(PagingRequest pagingRequest)
        {
            throw new NotImplementedException();
        }

        virtual public async Task<T> Update(T model)
        {
            throw new NotImplementedException();
        }
    }
}
