using GCatcode.Repository.DB.UserRolServices;
using GCatcode.Repository.DB.UserServices;
using GCatcode.Utils;
using GCatcode.Utils.extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GCatcode.Api.Core
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : BaseController
    {
        public UsersController(IConfiguration configuration, IUserService userService, IUserRolService userRolService)
            : base(configuration, userService, userRolService)
        {
        }

        [HttpGet, Route("")]
        public ActionResult<List<UserDTO>> Get(int page = 1, bool available = true)
        {
            try
            {
                if (!IsAdmin)
                    return Unauthorized();

                if (!available)
                    return Ok(_userService.Get(page: page));
                return Ok(_userService.Get(page: page, filters: new { Available = 1 }));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet, Route("{id}")]
        public ActionResult<UserAndRols> GetById(int id)
        {
            try
            {
                UserDTO userDto = _userService.GetById(id);
                UserAndRols userAndRols = new UserAndRols
                {
                    UserId = userDto.UserId,
                    UserName = userDto.UserName,
                    Email = userDto.Email,
                    Roles = _userRolService.GetRolsByUserId(id)
                };
                return Ok(userAndRols);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost, Route("")]
        public ActionResult<UserDTO> Post([FromBody] UserInsert data)
        {
            try
            {
                var res = _userService.Add(data);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut, Route("")]
        public ActionResult<UserDTO> Put([FromBody] UserUpdate data)
        {
            try
            {
                var res = _userService.Update(data);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut, Route("ChangePassword")]
        public ActionResult<UserDTO> ChangePassword([FromBody] UserChangePassword data)
        {
            try
            {
                var res = _userService.ChangePassword(data);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete, Route("{id}")]
        public ActionResult<UserDTO> Delete(int id)
        {
            if (!IsAdmin)
                return Unauthorized();
            try
            {
                if (id == GetUserId())
                    return BadRequest("You cannot delete your own user");
                var res = _userService.Delete(id);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
