using GCatcode.Api.Core;
using GCatcode.Repository.DB.RolServices;
using Microsoft.AspNetCore.Mvc;

namespace GCatcode.Api.Controllers
{
    [ApiController, Route("api/[controller]")]
    public class RolesController : BaseController
    {
        public RolesController(IConfiguration configuration)
            : base(configuration)
        {
        }

        [HttpGet, Route("")]
        public ActionResult<List<RolDTO>> Get()
        {
            try
            {
                var service = new RolesService(_connection);
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
                var service = new RolesService(_connection);
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
            using (var transaction = _connection.BeginTransaction())
            {
                try
                {
                    if (data.RolId >= 0)
                    {
                        var service = new RolesService(_connection);
                        var res = service.Update(data);
                        transaction.Commit();
                        return Ok(res);
                    }
                    else return BadRequest("RolId is required");

                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return BadRequest(ex.Message);
                }
            }
        }

        [HttpDelete, Route("{id}")]
        public ActionResult Del(int id)
        {
            using (var transaction = _connection.BeginTransaction())
            {
                try
                {
                    var service = new RolesService(_connection);
                    service.Delete(id);
                    transaction.Commit();
                    return Ok("Success");
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return BadRequest(ex.Message);
                }
            }
        }
    }
}