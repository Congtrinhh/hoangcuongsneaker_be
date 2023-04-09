using HoangCuongSneaker.Core;
using HoangCuongSneaker.Core.Dto;
using HoangCuongSneaker.Core.Dto.Paging.Admin;
using HoangCuongSneaker.Core.Model;
using HoangCuongSneaker.Core.Model.Admin.Product;
using HoangCuongSneaker.Repository;
using HoangCuongSneaker.Repository.Admin.Implementation;
using HoangCuongSneaker.Repository.Admin.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HoangCuongSneaker.Api.Controllers.Admin
{
    public class AdminProductInventoriesController : BaseController<ProductInventory>
    {
        protected IProductInventoryRepository _productInventoryRepository;
        public AdminProductInventoriesController(IProductInventoryRepository productInventoryRepository) : base(productInventoryRepository)
        {
            _productInventoryRepository = productInventoryRepository;
        }

        [HttpPost("paging")]
        public async Task<ApiResponse> GetProductInventories([FromBody] ProductInventoryPagingRequest pagingRequest)
        {
            var response = new ApiResponse();
            try
            {
                PagingResponse<ProductInventoryDto> pagingResponse = await _productInventoryRepository.GetPaging(pagingRequest);
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
