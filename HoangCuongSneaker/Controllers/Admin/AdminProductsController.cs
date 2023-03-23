using HoangCuongSneaker.Core;
using HoangCuongSneaker.Core.Dto;
using HoangCuongSneaker.Core.Dto.Paging.Admin;
using HoangCuongSneaker.Core.Model;
using HoangCuongSneaker.Core.Model.Admin.Product;
using HoangCuongSneaker.Core.Model.FileUpload;
using HoangCuongSneaker.Repository;
using HoangCuongSneaker.Repository.Admin.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HoangCuongSneaker.Api.Controllers.Admin
{
    public class AdminProductsController : BaseController<ProductDto>
    {
        private IImageRepository _imageRepository;
        public AdminProductsController(IProductRepository productRepository, IImageRepository imageRepository) : base(productRepository)
        {
            _imageRepository = imageRepository;
        }

        [HttpPost]
        [Route("images/{productId}")]
        public async Task<ApiResponse> CreateImages([FromForm] IFormFile file, int productId)
        {
            int count = 0;
            using (var ms = new MemoryStream())
            {
                file.CopyTo(ms);
                var fileBytes = ms.ToArray();
                string s = Convert.ToBase64String(fileBytes);

                var image = new Image();
                image.Content = fileBytes;
                image.ProductId = productId;
                var createdImage = await _imageRepository.Create(image);
                if (createdImage is not null && createdImage.Id > 0)
                {
                    count++;
                }
                var response = new ApiResponse();
                response.Data.Add(count);
                return response;
            }
        }

        [HttpGet("by-slug/{slug}")]
        public async Task<ApiResponse> GetBySlug(string slug)
        {
            var response = new ApiResponse();
            try
            {
                var product =  await ((IProductRepository)_baseRepository).GetBySlug(slug);
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
                PagingResponse<ProductDto> pagingResponse = await _baseRepository.GetPaging(pagingRequest);
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
