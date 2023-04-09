using HoangCuongSneaker.Core.Model;
using HoangCuongSneaker.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HoangCuongSneaker.Api.Controllers.Admin
{ 
    public class AdminBrandsController : BaseController<Brand>
    {
        public AdminBrandsController(IBaseRepository<Brand> baseRepository) : base(baseRepository)
        {
        }
    }
}
