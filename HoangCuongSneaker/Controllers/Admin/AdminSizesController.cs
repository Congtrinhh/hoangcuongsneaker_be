using HoangCuongSneaker.Core.Model;
using HoangCuongSneaker.Repository;
using HoangCuongSneaker.Repository.Admin.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HoangCuongSneaker.Api.Controllers.Admin
{
    public class AdminSizesController : BaseController<Size>
    {
        protected ISizeRepository _sizeRepository;
        public AdminSizesController(ISizeRepository sizeRepository) : base(sizeRepository)
        {
            _sizeRepository = sizeRepository;
        }
    }
}
