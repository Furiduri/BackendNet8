using GCatcode.Api.Configuration;
using GCatcode.Api.Core;
using GCatcode.Repository.DB.UserRolServices;
using GCatcode.Repository.DB.UserServices;
using Microsoft.AspNetCore.Mvc;

namespace GCatcode.Api.Controllers
{
    [ApiController, Route("api/[controller]")]
    public class TestController : BaseController
    {
        public TestController(AppSettings configuration)
            : base(configuration)
        {
        }

        [HttpGet, Route("")]
        public ActionResult Get()
        {
            try
            {
                ApiResponse response = ApiResponse.SuccessResult($"Holi {GetUserName()}");
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