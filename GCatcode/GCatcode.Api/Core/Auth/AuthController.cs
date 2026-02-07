using GCatcode.Api.Configuration;
using GCatcode.Api.Core.Auth.Models;
using GCatcode.Repository.DB.UserServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GCatcode.Api.Core.Auth
{
    [ApiController]
    [Route("api/[controller]")]
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
        [ProducesResponseType<UserInfo>(StatusCodes.Status200OK)]
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
        [ProducesResponseType<LoginInfo>(StatusCodes.Status200OK)]
        public ActionResult Login([FromBody] UserLogin user)
        {
            try
            {
                var response = _methods.Login(user);
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
        [ProducesResponseType<LoginInfo>(StatusCodes.Status200OK)]
        public IActionResult Register([FromBody] UserInsert user)
        {
            try
            {
                var response = _methods.Register(user);
                return StatusCode(response.StatusCode, response);
            }
            catch (Exception ex)
            {
                LogError(ex);
                var response = ApiResponse<string>.ErrorResult(AuthResponse.InternalServerError(ex.Message));
                return StatusCode((int)response.StatusCode, response);
            }
        }
    }
}