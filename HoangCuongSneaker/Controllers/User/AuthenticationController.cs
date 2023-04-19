using HoangCuongSneaker.Core.Dto;
using HoangCuongSneaker.Core.Model.Admin.Order;
using HoangCuongSneaker.Core.Model.Authentication;
using HoangCuongSneaker.Repository.Admin.Interface;
using HoangCuongSneaker.Repository.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HoangCuongSneaker.Api.Controllers.User
{
    /// <summary>
    /// Controller riêng cho phần login, nên không kế thừa từ base controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        protected IUserRepository _userRepository;
        protected IJwtRepository _jwtRepository;
        public AuthenticationController(IUserRepository userRepository, IJwtRepository jwtRepository)
        {
            _userRepository = userRepository;
            _jwtRepository = jwtRepository;
        }
        /// <summary>
        /// đăng nhập user / admin
        /// </summary>
        /// <returns></returns>
        [HttpPost("login")]
        public async Task<ApiResponse> Login([FromBody] UserLoginDto userLoginDto)
        {
            var response = new ApiResponse();
            try
            {
                var userFromDb = await _userRepository.GetByUserName(userLoginDto.UserName);
                if (userFromDb is null) throw new Exception("Không tìm thấy tài khoản");

                bool isPasswordMatch = BCrypt.Net.BCrypt.Verify(userLoginDto.Password, userFromDb.Password);
                if (!isPasswordMatch) throw new Exception("Mật khẩu không đúng");

                string jwtToken = _jwtRepository.GenerateToken(userFromDb);

                var responseData = new List<object> { jwtToken, userFromDb };
                response.OnSuccess(responseData);
            }
            catch (Exception e)
            {
                response.OnFailure(e);
            }
            return response;
        }

        /// <summary>
        /// đăng ký mới 1 người dùng
        /// </summary>
        /// <returns></returns>
        [HttpPost("registration")]
        public async Task<ApiResponse> Register(UserDto userRegistrationDto)
        {
            var apiResponse = new ApiResponse();
            try
            {
                var newlyCreatedUser = await _userRepository.Create(userRegistrationDto);

                if (newlyCreatedUser is null)
                {
                    throw new Exception("Tạo tài khoản thất bại");
                }

                apiResponse.OnSuccess(newlyCreatedUser);
            }
            catch (Exception e)
            {
                apiResponse.OnFailure(e);
            }
            return apiResponse;
        }
    }
}
