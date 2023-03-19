using HoangCuongSneaker.Core.Model.Admin.Product;
using HoangCuongSneaker.Repository;
using HoangCuongSneaker.Repository.Admin.Interface;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HoangCuongSneaker.Api.Controllers.Admin
{
    public class AdminProductsController : BaseController<ProductDto>
    {
        public AdminProductsController(IProductRepository productRepository) : base(productRepository)
        {
        }
    }
}
