﻿using HoangCuongSneaker.Core;
using HoangCuongSneaker.Core.Dto.Paging;
using HoangCuongSneaker.Core.Model;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoangCuongSneaker.Repository
{
    public interface IBaseRepository<T> where T : BaseModel
    {
        Task<T?> Get(int id, MySqlConnection connection = null);
        Task<List<T>> GetAll(MySqlConnection connection = null);
        //Task<PagingResponse<T>> GetPaging(PagingRequest pagingRequest, MySqlConnection connection = null);
        Task<T?> Create(T model, MySqlConnection connection = null);
        Task<T?> Update(T model, MySqlConnection connection = null);
        Task<int> Delete(int id, MySqlConnection connection = null);
    }
}
