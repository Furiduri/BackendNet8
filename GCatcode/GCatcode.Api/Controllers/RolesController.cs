using GCatcode.Api.Core;
using GCatcode.Repository.DB.RolService;
using Microsoft.AspNetCore.Mvc;

namespace GCatcode.Api.Controllers
{
    [ApiController, Route("api/[controller]")]
    public class RolesController : BaseController
    {
        private readonly RolesService service;

        public RolesController(IConfiguration configuration)
            : base(configuration)
        {
            service = new RolesService(_configuration.GetConnectionString("BaseLine"));
        }

        [HttpGet, Route("")]
        public ActionResult<List<RolDTO>> Get()
        {
            try
            {
                return Ok(service.Get());
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
                return Ok(service.GetById(id));
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
                    return Ok(service.Update(data));
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
                service.Delete(id);
                return Ok("Success");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}