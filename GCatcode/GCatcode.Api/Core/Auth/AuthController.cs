using GCatcode.Api.Configuration;
using GCatcode.Api.Core.Auth.Models;
using GCatcode.Services.DB.UserServices;
using GCatcode.Utils.GenericModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GCatcode.Api.Core.Auth
{
    [ApiController, Route("api/[controller]")]
    public class AuthController : BaseController
    {
        private readonly AuthMethods _methods;

        public AuthController(AppSettings configuration)
            : base(configuration)
        {
            _methods = new AuthMethods(configuration);
        }

        [Authorize]
        [HttpGet("user_info")]
        [ProducesResponseType<ApiResponse<UserInfo>>(StatusCodes.Status200OK)]
        public ActionResult GetUserInfo()
        {
            try
            {
                var response = _methods.GetUserInfo(GetUserId());
                return StatusCode(response.StatusCode, response);
            }
            catch (Exception ex)
            {
                LogError(ex);
                var response = ApiResponse<string>.ErrorResult(AuthResponse.InternalServerError(ex.Message));
                return StatusCode((int)response.StatusCode, response);
            }
        }

        /// <summary>
        /// Login user and generate JWT token
        /// </summary>
        /// <param name="user">user and Password in string Base64</param>
        /// <returns>JWT token</returns>
        [HttpPost("login")]
        [ProducesResponseType<ApiResponse<LoginInfo>>(StatusCodes.Status200OK)]
        public ActionResult Login([FromBody] UserLogin user)
        {
            try
            {
                var ipAddress = GetIpAddress();
                var response = _methods.Login(user, ipAddress);
                return StatusCode(response.StatusCode, response);
            }
            catch (Exception ex)
            {
                LogError(ex);
                var response = ApiResponse<string>.ErrorResult(AuthResponse.InternalServerError(ex.Message));
                return StatusCode((int)response.StatusCode, response);
            }
        }

        /// <summary>
        /// Register New User
        /// </summary>
        /// <param name="user"> UserName, email and Password in string Base64</param>
        /// <returns>JWT token</returns>
        [HttpPost("register")]
        [ProducesResponseType<ApiResponse<LoginInfo>>(StatusCodes.Status200OK)]
        public IActionResult Register([FromBody] UserInsert user)
        {
            try
            {
                var ipAddress = GetIpAddress();
                var response = _methods.Register(user, ipAddress);
                return StatusCode(response.StatusCode, response);
            }
            catch (Exception ex)
            {
                LogError(ex);
                var response = ApiResponse<string>.ErrorResult(AuthResponse.InternalServerError(ex.Message));
                return StatusCode((int)response.StatusCode, response);
            }
        }

        /// <summary>
        /// Refresh access token using refresh token
        /// </summary>
        /// <param name="request">Refresh token</param>
        /// <returns>New JWT token and refresh token</returns>
        [HttpPost("refresh")]
        [ProducesResponseType<ApiResponse<LoginInfo>>(StatusCodes.Status200OK)]
        public IActionResult RefreshToken([FromBody] RefreshTokenRequest request)
        {
            try
            {
                var ipAddress = GetIpAddress();
                var response = _methods.RefreshToken(request.RefreshToken, ipAddress);
                return StatusCode(response.StatusCode, response);
            }
            catch (Exception ex)
            {
                LogError(ex);
                var response = ApiResponse<string>.ErrorResult(AuthResponse.InternalServerError(ex.Message));
                return StatusCode((int)response.StatusCode, response);
            }
        }

        /// <summary>
        /// Revoke refresh token
        /// </summary>
        /// <param name="request">Refresh token to revoke</param>
        /// <returns>Success message</returns>
        [HttpPost("revoke")]
        [ProducesResponseType<ApiResponse<GenericMessage>>(StatusCodes.Status200OK)]
        public IActionResult RevokeToken([FromBody] RefreshTokenRequest request)
        {
            try
            {
                var ipAddress = GetIpAddress();
                var response = _methods.RevokeToken(request.RefreshToken, ipAddress);
                return StatusCode(response.StatusCode, response);
            }
            catch (Exception ex)
            {
                LogError(ex);
                var response = ApiResponse<string>.ErrorResult(AuthResponse.InternalServerError(ex.Message));
                return StatusCode((int)response.StatusCode, response);
            }
        }

        private string GetIpAddress()
        {
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
                return Request.Headers["X-Forwarded-For"];

            return HttpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString() ?? "unknown";
        }
    }
}