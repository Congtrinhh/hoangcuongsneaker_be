using HoangCuongSneaker.Core.Model;
using HoangCuongSneaker.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HoangCuongSneaker.Api.Controllers.Admin
{
    public class AdminSizesController : BaseController<Size>
    {
        public AdminSizesController(IBaseRepository<Size> baseRepository) : base(baseRepository)
        {
        }
    }
}
