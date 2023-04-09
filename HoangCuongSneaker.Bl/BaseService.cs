using AutoMapper;
using HoangCuongSneaker.Core.Model;
using HoangCuongSneaker.Core.Model.Admin.Order;
using HoangCuongSneaker.Core.Utility;
using HoangCuongSneaker.Repository;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoangCuongSneaker.Service
{
    public class BaseService<T>:IBaseService<T> where T : BaseModel
    {
        protected IMapper _mapper;
        protected IBaseRepository<T> _baseRepository;
        protected string _connectionString = string.Empty;
        protected readonly IConfiguration _configuration;

        public BaseService(IConfiguration configuration, IBaseRepository<T> baseRepository)
        {
            _baseRepository = baseRepository;
            _mapper = MapperUtil.Initialize();
            _configuration = configuration;
            _connectionString = _configuration["ConnectionStrings:DefaultConnection"] ?? string.Empty;
        } 

        public MySqlConnection GetSqlConnection()
        {
            return new MySqlConnection(_connectionString);
        }

        protected virtual async Task BeforeSave(T model)
        {
            
        }
        protected virtual async Task AfterSave(T model)
        {

        }

    }
}
