using HoangCuongSneaker.Core.Dto;
using HoangCuongSneaker.Core.Dto.Paging.AdminStatistic;
using HoangCuongSneaker.Repository.AdminStatistic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HoangCuongSneaker.Api.Controllers.AdminStatistic
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AdminStatisticController : ControllerBase
    {
        protected IAdminStatisticRepository _adminStatisticRepository;
        public AdminStatisticController(IAdminStatisticRepository adminStatisticRepository)
        {
            _adminStatisticRepository = adminStatisticRepository;
        }

        [HttpPost("admin-statistic")]
        public async Task<ApiResponse> GetProducts(StatisticProductPagingRequest pagingRequest)
        {
            var response = new ApiResponse();
            try
            {
                var product = await _adminStatisticRepository.GetProducts(pagingRequest);
                response.OnSuccess(data: product);
            }
            catch (Exception e)
            {
                response.OnFailure(e);
            }
            return response;
        }
    }
}
