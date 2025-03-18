using GCatcode.Api.Core;
using GCatcode.Repository.DB;
using GCatcode.SQLServerDatabase.Dtos;
using GCatcode.SQLServerDatabase.Models;
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
        public ActionResult<List<Rol>> Get()
        {
            try
            {
                return Ok(service.Get(filters: new { Available = true }));
            }
            catch (Exception ex)
            {
               return BadRequest(ex.Message);
            }
        }

        [HttpGet, Route("{id}")]
        public ActionResult<Rol> GetById(int id)
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
        public ActionResult<Rol> Post([FromBody] RolDTO data)
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
        public ActionResult<Rol> Put([FromBody] RolDTO data)
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
