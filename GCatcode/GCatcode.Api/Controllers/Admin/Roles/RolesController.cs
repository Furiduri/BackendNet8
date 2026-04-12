using GCatcode.Api.Configuration;
using GCatcode.Api.Core;
using GCatcode.Api.Core.Authorization;
using GCatcode.Services.DB.RolServices.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GCatcode.Api.Controllers.Admin.Roles
{
    [Authorize]
    [RequirePermission("roles.admin")]
    [ApiController, Route("api/admin/[controller]")]
    public class RolesController : BaseController
    {
        private readonly RolesMethods _methods;

        public RolesController(AppSettings configuration)
            : base(configuration)
        {
            _methods = new RolesMethods(configuration);
        }

        [HttpGet]
        [ProducesResponseType<List<RolDTO>>(StatusCodes.Status200OK)]
        public ActionResult Get(int page = 1)
        {
            try
            {
                var response = _methods.Get(page);
                return StatusCode(response.StatusCode, response);
            }
            catch (Exception ex)
            {
                LogError(ex);
                var response = ApiResponse<string>.ErrorResult(RolesResponse.InternalServerError(ex.Message));
                return StatusCode((int)response.StatusCode, response);
            }
        }

        [HttpGet, Route("{id}")]
        [ProducesResponseType<RolDTO>(StatusCodes.Status200OK)]
        public ActionResult GetById(int id)
        {
            try
            {
                var response = _methods.GetById(id);
                return StatusCode(response.StatusCode, response);
            }
            catch (Exception ex)
            {
                LogError(ex);
                var response = ApiResponse<string>.ErrorResult(RolesResponse.InternalServerError(ex.Message));
                return StatusCode((int)response.StatusCode, response);
            }
        }

        [HttpPut, Route("")]
        [ProducesResponseType<RolDTO>(StatusCodes.Status200OK)]
        public ActionResult Put([FromBody] RolUpdate data)
        {
            try
            {
                var response = _methods.Update(data);
                return StatusCode(response.StatusCode, response);
            }
            catch (Exception ex)
            {
                LogError(ex);
                var response = ApiResponse<string>.ErrorResult(RolesResponse.InternalServerError(ex.Message));
                return StatusCode((int)response.StatusCode, response);
            }
        }

        [HttpDelete, Route("{id}")]
        [ProducesResponseType<int>(StatusCodes.Status200OK)]
        public ActionResult Del(int id)
        {
            try
            {
                var response = _methods.Delete(id);
                return StatusCode(response.StatusCode, response);
            }
            catch (Exception ex)
            {
                LogError(ex);
                var response = ApiResponse<string>.ErrorResult(RolesResponse.InternalServerError(ex.Message));
                return StatusCode((int)response.StatusCode, response);
            }
        }
    }
}