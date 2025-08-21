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
        public UsersController(IConfiguration configuration)
            : base(configuration)
        {
        }

        [HttpGet, Route("")]
        public ActionResult<List<UserDTO>> Get(int page = 1, bool available = true)
        {
            try
            {
                if (!IsAdmin)
                    return Unauthorized();

                var service = new UserService(_connection);
                if (!available)
                    return Ok(service.Get(page: page));
                return Ok(service.Get(page: page, filters: new { Available = 1 }));
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
                var service = new UserService(_connection);
                var userRolService = new UserRolService(_connection);
                UserDTO userDto = service.GetById(id);
                UserAndRols userAndRols = new UserAndRols
                {
                    UserId = userDto.UserId,
                    UserName = userDto.UserName,
                    Email = userDto.Email,
                    Roles = userRolService.GetRolsByUserId(id)
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
            using (var transaction = _connection.BeginTransaction())
            {
                try
                {
                    var service = new UserService(_connection);
                    var res = service.Add(data);
                    transaction.Commit();
                    return Ok(res);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return BadRequest(ex.Message);
                }
            }
        }

        [HttpPut, Route("")]
        public ActionResult<UserDTO> Put([FromBody] UserUpdate data)
        {
            using (var transaction = _connection.BeginTransaction())
            {
                try
                {
                    var service = new UserService(_connection);
                    var res = service.Update(data);
                    transaction.Commit();
                    return Ok(res);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return BadRequest(ex.Message);
                }
            }
        }

        [HttpPut, Route("ChangePassword")]
        public ActionResult<UserDTO> ChangePassword([FromBody] UserChangePassword data)
        {
            using (var transaction = _connection.BeginTransaction())
            {
                try
                {
                    if (!TripleDESHelper.Decrypt(data.NewPassword).ValidatePassword())
                        return BadRequest("Invalid password, use lowercase, uppercase, numbers and symbols, min length 8.");

                    var service = new UserService(_connection);
                    if (!service.ValidPassword(new UserLogin { Username = data.UserName, Password = data.OldPassword }))
                        return BadRequest("Invalid password");
                    var res = service.ChangePassword(data);
                    transaction.Commit();
                    return Ok(res);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return BadRequest(ex.Message);
                }
            }
        }

        [HttpDelete, Route("{id}")]
        public ActionResult<UserDTO> Delete(int id)
        {
            if (!IsAdmin)
                return Unauthorized();
            using (var transaction = _connection.BeginTransaction())
            {
                try
                {
                    var service = new UserService(_connection);
                    if (id == GetUserId())
                        return BadRequest("You cannot delete your own user");
                    var res = service.Delete(id);
                    transaction.Commit();
                    return Ok(res);
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