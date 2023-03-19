using HoangCuongSneaker.Core.Model.Admin.Order;
using HoangCuongSneaker.Repository;
using HoangCuongSneaker.Repository.Admin.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HoangCuongSneaker.Api.Controllers.Admin
{
    public class AdminUsersController : BaseController<UserDto>
    {
        public AdminUsersController(IUserRepository userRepository) : base(userRepository)
        {
        }



    }
}
