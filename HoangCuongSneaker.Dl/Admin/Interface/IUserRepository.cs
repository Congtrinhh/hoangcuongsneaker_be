using HoangCuongSneaker.Core;
using HoangCuongSneaker.Core.Dto.Paging;
using HoangCuongSneaker.Core.Model;
using HoangCuongSneaker.Core.Model.Admin.Order;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoangCuongSneaker.Repository.Admin.Interface
{
    public interface IUserRepository: IBaseRepository<UserDto>
    {
        Task<UserDto?> GetByUserName(string userName, MySqlConnection connection = null);
        Task<PagingResponse<UserDto>> GetPaging(PagingRequest pagingRequest, MySqlConnection connection = null);
    }
}
