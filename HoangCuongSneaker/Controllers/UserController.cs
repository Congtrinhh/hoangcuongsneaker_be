using HoangCuongSneaker.Core.Model;
using HoangCuongSneaker.Repository;
using HoangCuongSneaker.Repository.Admin.Interface;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HoangCuongSneaker.Api.Controllers
{
    public class UserController : BaseController<User>
    { 
        public UserController(IBaseRepository<User> baseRepository) : base(baseRepository)
        {
        }
    }
}
