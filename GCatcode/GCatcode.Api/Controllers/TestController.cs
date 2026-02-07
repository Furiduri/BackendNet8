using GCatcode.Api.Configuration;
using GCatcode.Api.Core;
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
        [ProducesResponseType<string>(StatusCodes.Status200OK)]
        public ActionResult Get()
        {
            try
            {
                var response = ApiResponse<string>.SuccessResult($"Holi {GetUserName()}");
                return StatusCode(response.StatusCode, response);
            }
            catch (Exception ex)
            {
                LogError(ex);
                var response = BaseResponse.InternalServerError(ex.Message);
                return StatusCode((int)response.StatusCode, response);
            }
        }
    }
}