using Dapper;
using HoangCuongSneaker.Core;
using HoangCuongSneaker.Core.Dto.Paging;
using HoangCuongSneaker.Core.Model;
using HoangCuongSneaker.Core.Utility;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;


namespace HoangCuongSneaker.Repository
{
    public class BaseRepository<T> : IBaseRepository<T> where T : BaseModel
    {
        #region Property
        protected readonly IConfiguration _configuration;
        protected string _tableName = string.Empty;
        protected string _connectionString = string.Empty;
        protected IMapper _mapper;
        #endregion

        #region Contructor
        public BaseRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _tableName = FunctionUtil.ToSnakeCase(typeof(T).Name);
            _connectionString = _configuration["ConnectionStrings:DefaultConnection"] ?? string.Empty;
            _mapper = MapperUtil.Initialize();
        }
        #endregion

        #region Method


        public MySqlConnection GetSqlConnection()
        {
            return new MySqlConnection(_connectionString);
        }

        /// <summary>
        /// tạo model và trả về model đó
        /// </summary>
        /// <param name="model"></param>
        /// <param name="connection"></param>
        /// <returns></returns>
        public virtual async Task<T?> Create(T model, MySqlConnection connection = null)
        {
            connection = connection ?? GetSqlConnection();
            connection.Open();
            var transaction = connection.BeginTransaction();

            string procInsert = $"proc_{_tableName}_insert";
            var insertedId = await connection.ExecuteScalarAsync<int>(sql: procInsert, commandType: System.Data.CommandType.StoredProcedure, param: model, transaction: transaction);
            if (insertedId != 0)
            {
                var newlyInsertedModel = await Get(insertedId);
                return newlyInsertedModel;
            }
            return null;
        }

        /// <summary>
        /// xoá model dựa theo khoá chính. 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="connection"></param>
        /// <returns>Nếu xoá thành công, trả về 1; Nếu xoá thất bại, trả về 0</returns>
        public virtual async Task<int> Delete(int id, MySqlConnection connection = null)
        {
            connection = connection ?? GetSqlConnection();
            connection.Open();
            string sqlDelete = $"delete from {_tableName} where id=@id";
            var res = await connection.ExecuteAsync(sql: sqlDelete, param: new { @id = id }, commandType: System.Data.CommandType.Text);
            return res;
        }

        /// <summary>
        /// lấy ra tất cả model 
        /// </summary>
        /// <param name="connection"></param>
        /// <returns></returns>
        public virtual async Task<List<T>> GetAll(MySqlConnection connection = null)
        {
            connection = connection ?? GetSqlConnection();

            string sql = $"select * from {_tableName}";
            var models = await connection.QueryAsync<T>(sql: sql, commandType: System.Data.CommandType.Text);
            if (models is not null)
            {
                return models.ToList();
            }

            return new List<T>();
        }

        /// <summary>
        /// lấy ra model dựa vào khoá chính (id) của nó
        /// </summary>
        /// <param name="id"></param>
        /// <param name="connection"></param>
        /// <returns></returns>
        public virtual async Task<T?> Get(int id, MySqlConnection connection = null)
        {
            connection = connection ?? GetSqlConnection();

            string sql = $"select * from {_tableName} where id=@id";
            var paramDictionary = new DynamicParameters();
            paramDictionary.Add("@id", id);
            var res = await connection.QueryFirstOrDefaultAsync<T>(sql: sql, param: paramDictionary, commandType: System.Data.CommandType.Text);
            return res;
        }

        /// <summary>
        /// tạo câu lệnh get paging
        /// </summary>
        /// <param name="pagingRequest"></param>
        /// <returns></returns>
        protected virtual string GetSqlGetPaging(PagingRequest pagingRequest)
        {

            var sql = $"select * from {_tableName} where 1=1";

            int limit = pagingRequest.PageSize;
            int offset = pagingRequest.PageSize * pagingRequest.PageIndex;
            sql += $" limit {limit} offset {offset}";

            return sql;
        }

        /// <summary>
        /// cập nhật model và trả về model đó với giá trị mới cập nhật
        /// </summary>
        /// <param name="model"></param>
        /// <param name="connection"></param>
        /// <returns></returns>
        public virtual async Task<T?> Update(T model, MySqlConnection connection = null)
        {
            connection = connection ?? GetSqlConnection();
            connection.Open();
            var transaction = connection.BeginTransaction();

            var procedureUpdate = $"proc_{_tableName}_update";
            var res = await connection.ExecuteAsync(sql: procedureUpdate, param: model, commandType: System.Data.CommandType.StoredProcedure, transaction: transaction);
            if (res != 0)
            {
                var newlyUpdatedModel = await Get(model.Id);
                return newlyUpdatedModel;
            }
            return null;
        }

        #endregion
    }
}
