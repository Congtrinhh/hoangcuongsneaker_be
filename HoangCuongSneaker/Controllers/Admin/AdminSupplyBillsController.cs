using HoangCuongSneaker.Core.Dto;
using HoangCuongSneaker.Core.Dto.Paging.Admin;
using HoangCuongSneaker.Core.Model.Admin.SupplyBill;
using HoangCuongSneaker.Repository;
using HoangCuongSneaker.Repository.Admin.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HoangCuongSneaker.Api.Controllers.Admin
{
    [Authorize(Roles = "admin")]
    public class AdminSupplyBillsController : BaseController<SupplyBillDto>
    {
        protected ISupplyBillRepository _supplyBillRepository;
        public AdminSupplyBillsController(ISupplyBillRepository supplyBillRepository) : base(supplyBillRepository)
        {
            _supplyBillRepository = supplyBillRepository;
        }

        [HttpPost("paging")]
        public async Task<ApiResponse> GetPaging(SupplyBillPagingRequest pagingRequest)
        {
            var response = new ApiResponse();
            try
            {
                var pagingResponse = await _supplyBillRepository.GetPaging(pagingRequest);
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
