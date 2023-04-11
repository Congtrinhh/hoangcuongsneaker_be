using HoangCuongSneaker.Core;
using HoangCuongSneaker.Core.Dto.Paging;
using HoangCuongSneaker.Core.Model.Admin.Order;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoangCuongSneaker.Repository.Admin.Interface
{
    public interface IOrderRepository:IBaseRepository<OrderDto>
    {
        public Task<PagingResponse<OrderDto>> GetPaging(PagingRequest pagingRequest, MySqlConnection connection = null);
    }
}
