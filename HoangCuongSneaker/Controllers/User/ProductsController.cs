using HoangCuongSneaker.Core.Dto;
using HoangCuongSneaker.Core.Dto.Paging;
using HoangCuongSneaker.Core.Model.Admin.Product;
using HoangCuongSneaker.Repository;
using HoangCuongSneaker.Repository.Admin.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HoangCuongSneaker.Api.Controllers.User
{ 
    public class ProductsController : BaseController<ProductDto>
    {
        public ProductsController(IProductRepository productRepository) : base(productRepository)
        {
        }
    }
}
