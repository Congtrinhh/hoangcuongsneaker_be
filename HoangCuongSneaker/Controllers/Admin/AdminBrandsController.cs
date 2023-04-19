using HoangCuongSneaker.Core.Model;
using HoangCuongSneaker.Repository;
using HoangCuongSneaker.Repository.Admin;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HoangCuongSneaker.Api.Controllers.Admin
{ 
    public class AdminBrandsController : BaseController<Brand>
    {
        protected IBrandRepository _brandRepository;
        public AdminBrandsController(IBrandRepository brandRepository) : base(brandRepository)
        {
            _brandRepository = brandRepository;
        }
    }
}
