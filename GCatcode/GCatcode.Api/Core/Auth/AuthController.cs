using GCatcode.Api.Configuration;
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
        public ActionResult GetUserInfo()
        {
            try
            {
                ApiResponse response = _methods.GetUserInfo(GetUserId());
                if (response.Success)
                {
                    return Ok(response);
                }
                else
                {
                    return BadRequest(response);
                }
            }
            catch (Exception ex)
            {
                LogError(ex);
                return BadRequest($"{ResponseMessageCommon.InternalServerError.ToMsgString()} : {ex.Message}");
            }
        }

        [HttpPost("login")]
        public ActionResult Login([FromBody] UserLogin user)
        {
            try
            {
                ApiResponse response = _methods.Login(user);
                if (response.Success)
                {
                    return Ok(response);
                }
                else
                {
                    return BadRequest(response);
                }
            }
            catch (Exception ex)
            {
                LogError(ex);
                return BadRequest($"{ResponseMessageCommon.InternalServerError.ToMsgString()} : {ex.Message}");
            }
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] UserInsert user)
        {
            try
            {
                ApiResponse response = _methods.Register(user);
                if (response.Success)
                {
                    return Ok(response);
                }
                else
                {
                    return BadRequest(response);
                }
            }
            catch (Exception ex)
            {
                LogError(ex);
                return BadRequest($"{ResponseMessageCommon.InternalServerError.ToMsgString()} : {ex.Message}");
            }
        }
    }
}