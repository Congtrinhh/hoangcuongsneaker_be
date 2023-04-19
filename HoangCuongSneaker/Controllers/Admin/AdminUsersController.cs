using HoangCuongSneaker.Core.Dto;
using HoangCuongSneaker.Core.Dto.Paging.Admin;
using HoangCuongSneaker.Core.Enum;
using HoangCuongSneaker.Core.Model.Admin.Order;
using HoangCuongSneaker.Repository;
using HoangCuongSneaker.Repository.Admin.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HoangCuongSneaker.Api.Controllers.Admin
{
    public class AdminUsersController : BaseController<UserDto>
    {
        protected IUserRepository _userRepository;
        public AdminUsersController(IUserRepository userRepository) : base(userRepository)
        {
            _userRepository = userRepository;
        }

        [Authorize(Roles = "admin") ]
        [HttpPost("paging")]
        public async Task<ApiResponse> GetPaging(UserPagingRequest pagingRequest)
        {
            var response = new ApiResponse();
            try
            {
                var pagingResponse = await _userRepository.GetPaging(pagingRequest);
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
