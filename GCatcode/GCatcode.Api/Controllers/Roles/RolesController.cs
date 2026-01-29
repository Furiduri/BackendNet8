using GCatcode.Api.Configuration;
using GCatcode.Api.Core;
using GCatcode.Repository.DB.RolServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace GCatcode.Api.Controllers.Roles
{
    [Authorize]
    [ApiController, Route("api/[controller]")]
    public class RolesController : BaseController
    {
        private readonly RolesMethods _methods;

        public RolesController(AppSettings configuration)
            : base(configuration)
        {
            _methods = new RolesMethods(configuration);
        }

        [HttpGet]
        public ActionResult<List<RolDTO>> Get(int page = 1)
        {
            try
            {
                ApiResponse response = _methods.Get(page);
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

        [HttpGet, Route("{id}")]
        public ActionResult<RolDTO> GetById(int id)
        {
            try
            {
                ApiResponse response = _methods.GetById(id);
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

        [HttpPut, Route("")]
        public ActionResult<RolDTO> Put([FromBody] RolUpdate data)
        {
            try
            {
                ApiResponse response = _methods.Update(data);
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

        [HttpDelete, Route("{id}")]
        public ActionResult Del(int id)
        {
            try
            {
                ApiResponse response = _methods.Delete(id);
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