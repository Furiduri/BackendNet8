using GCatcode.Api.Core;
using GCatcode.Repository.DB.UserRolServices;
using GCatcode.Repository.DB.UserServices;
using Microsoft.AspNetCore.Mvc;

namespace GCatcode.Api.Controllers
{
    [ApiController, Route("api/[controller]")]
    public class TestController : BaseController
    {
        public TestController(IConfiguration configuration, IUserService userService, IUserRolService userRolService)
            : base(configuration, userService, userRolService)
        {
        }

        [HttpGet, Route("")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult<string> Get()
        {
            try
            {
                return Ok("Holi");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet, Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult<int> GetById(int id)
        {
            try
            {
                return Ok(id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost, Route("")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult Post([FromBody] string data)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                if (data == null)
                    return BadRequest();

                return StatusCode(201, data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut, Route("")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult Put([FromBody] string data)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                if (data == null)
                    return BadRequest();

                return StatusCode(200, data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete, Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult Del(int id)
        {
            try
            {
                if (id < 0)
                    return BadRequest();

                return Ok(id);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}