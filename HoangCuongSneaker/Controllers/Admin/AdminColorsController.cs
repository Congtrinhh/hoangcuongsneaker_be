using HoangCuongSneaker.Core.Model;
using HoangCuongSneaker.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HoangCuongSneaker.Api.Controllers.Admin
{
    public class AdminColorsController : BaseController<Color>
    {
        public AdminColorsController(IBaseRepository<Color> baseRepository) : base(baseRepository)
        {
        }
    }
}
