using HoangCuongSneaker.Core;
using HoangCuongSneaker.Core.Dto;
using HoangCuongSneaker.Core.Dto.Paging.Admin;
using HoangCuongSneaker.Core.Model;
using HoangCuongSneaker.Core.Model.Admin.Product; 
using HoangCuongSneaker.Repository;
using HoangCuongSneaker.Repository.Admin.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HoangCuongSneaker.Api.Controllers.Admin
{
    public class AdminProductsController : BaseController<ProductDto>
    {
        protected IImageRepository _imageRepository;
        protected IProductRepository _productRepository;
        public AdminProductsController(IProductRepository productRepository, IImageRepository imageRepository) : base(productRepository)
        {
            _imageRepository = imageRepository;
            _productRepository = productRepository;
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        [Route("images/{productId}")]
        public async Task<ApiResponse> CreateImages([FromForm] List<IFormFile> files, int productId)
        {
            // nhận list file thông qua đối tượng request
            var filesFromRequest = HttpContext.Request.Form.Files;

            var response = new ApiResponse();
            try
            {
                int affectedRows = await _productRepository.CreateImages(productId, filesFromRequest);
                response.OnSuccess(data: affectedRows);
            }
            catch (Exception e)
            {
                response.OnFailure(e);
            }
            return response; 
        }

        [HttpGet("by-slug/{slug}")]
        public async Task<ApiResponse> GetBySlug(string slug)
        {
            var response = new ApiResponse();
            try
            {
                var product = await ((IProductRepository)_baseRepository).GetBySlug(slug);
                response.OnSuccess(data: product);
            }
            catch (Exception e)
            {
                response.OnFailure(e);
            }
            return response;
        }

        [HttpPost("paging")]
        public async Task<ApiResponse> GetPaging(ProductPagingRequest pagingRequest)
        {
            var response = new ApiResponse();
            try
            {
                PagingResponse<ProductDto> pagingResponse = await _productRepository.GetPaging(pagingRequest);
                response.OnSuccess(data: pagingResponse);
            }
            catch (Exception e)
            {
                response.OnFailure(e);
            }
            return response;
        } 
    }
}
