using HoangCuongSneaker.Core.Model;
using HoangCuongSneaker.Repository;
using HoangCuongSneaker.Repository.Admin.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HoangCuongSneaker.Api.Controllers.Admin
{
    public class AdminColorsController : BaseController<Color>
    {
        protected IColorRepository _colorRepository;
        public AdminColorsController(IColorRepository colorRepository) : base(colorRepository)
        {
            _colorRepository = colorRepository;
        }
    }
}
