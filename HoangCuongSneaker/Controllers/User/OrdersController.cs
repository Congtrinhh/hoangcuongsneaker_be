
using HoangCuongSneaker.Core.Dto;
using HoangCuongSneaker.Core.Dto.Paging.Admin;
using HoangCuongSneaker.Core.Model.Admin.Order;
using HoangCuongSneaker.Repository.Admin.Interface;
using HoangCuongSneaker.Repository.Admin.Interface.Address;
using HoangCuongSneaker.Service.Admin;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HoangCuongSneaker.Api.Controllers.User
{
    public class OrdersController : BaseController<OrderDto>
    {
        protected IOrderService _orderService;
        protected IProvinceRepository _provinceRepository;
        protected IDistrictRepository _districtRepository;
        protected IWardRepository _wardRepository;
        protected IOrderRepository _orderRepository;
        public OrdersController(IOrderRepository orderRepository, IOrderService orderService, IProvinceRepository provinceRepository, IDistrictRepository districtRepository, IWardRepository wardRepository) : base(orderRepository)
        {
            _orderService = orderService;
            _provinceRepository = provinceRepository;
            _districtRepository = districtRepository;
            _wardRepository = wardRepository;
            _orderRepository = orderRepository;
        }

        [HttpGet("provinces")]
        public async Task<ApiResponse> GetProvinces()
        {
            var response = new ApiResponse();
            try
            {
                var provinces = await _provinceRepository.GetAll();
                response.OnSuccess(data: provinces);
            }
            catch (Exception e)
            {
                response.OnFailure(e);
            }
            return response;
        }

        [HttpGet("districts")]
        public async Task<ApiResponse> GetDistricts([FromQuery] int provinceId)
        {
            var response = new ApiResponse();
            try
            {
                var districts = await _districtRepository.GetDistricts(provinceId);
                response.OnSuccess(data: districts);
            }
            catch (Exception e)
            {
                response.OnFailure(e);
            }
            return response;
        }

        [HttpGet("wards")]
        public async Task<ApiResponse> GetWards([FromQuery] int districtId)
        {
            var response = new ApiResponse();
            try
            {
                var wards = await _wardRepository.GetWards(districtId);
                response.OnSuccess(data: wards);
            }
            catch (Exception e)
            {
                response.OnFailure(e);
            }
            return response;
        }

        [HttpPost]
        public async override Task<ApiResponse> Create([FromBody] OrderDto model)
        {
            // TODO: gen cac truong bo trong o day
            var response = new ApiResponse();
            try
            {
                var res = await _orderService.Create(model);
                response.OnSuccess(res);
            }
            catch (Exception e)
            {
                response.OnFailure(e);
            }
            return response;
        }

        [HttpPost("paging")]
        public async Task<ApiResponse> GetPaging([FromBody] OrderPagingRequest pagingRequest)
        {
            var response = new ApiResponse();
            try
            {
                var res = await _orderRepository.GetPaging(pagingRequest);
                response.OnSuccess(res);
            }
            catch (Exception e)
            {
                response.OnFailure(e);
            }
            return response;
        }
    }
}
