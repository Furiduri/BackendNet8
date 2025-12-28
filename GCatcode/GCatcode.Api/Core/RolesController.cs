using GCatcode.Repository.DB.RolServices;
using GCatcode.Repository.DB.UserRolServices;
using GCatcode.Repository.DB.UserServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GCatcode.Api.Core
{
    [Authorize]
    [ApiController, Route("api/[controller]")]
    public class RolesController : BaseController
    {
        private readonly IRolesService _rolesService;

        public RolesController(IConfiguration configuration, IUserService userService, IUserRolService userRolService, IRolesService rolesService)
            : base(configuration, userService, userRolService)
        {
            _rolesService = rolesService;
        }

        [HttpGet, Route("")]
        public ActionResult<List<RolDTO>> Get()
        {
            try
            {
                return Ok(_rolesService.Get());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet, Route("{id}")]
        public ActionResult<RolDTO> GetById(int id)
        {
            try
            {
                return Ok(_rolesService.GetById(id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost, Route("")]
        public ActionResult<RolDTO> Post([FromBody] RolUpdate data)
        {
            try
            {
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut, Route("")]
        public ActionResult<RolDTO> Put([FromBody] RolUpdate data)
        {
            try
            {
                if (data.RolId >= 0)
                {
                    var res = _rolesService.Update(data);
                    return Ok(res);
                }
                else return BadRequest("RolId is required");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete, Route("{id}")]
        public ActionResult Del(int id)
        {
            try
            {
                _rolesService.Delete(id);
                return Ok("Success");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}