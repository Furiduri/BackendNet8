using GCatcode.Api.Core;
using GCatcode.Repository.DB.RolService;
using GCatcode.Repository.DB.UserRolService;
using GCatcode.Repository.DB.UserService;
using GCatcode.Utils;
using GCatcode.Utils.extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GCatcode.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : BaseController
    {
        private readonly UserService service;
        private readonly UserRolService userRolService;
        public UsersController(IConfiguration configuration)
            :base(configuration)
        {
            service = new UserService(Settings.GetBaseDBConnection(_configuration));
            userRolService = new UserRolService(Settings.GetBaseDBConnection(_configuration));
        }

        [HttpGet, Route("")]
        public ActionResult<List<UserDTO>> Get(int page = 1, bool available = true)
        {
            try
            {
                if (!IsAdmin)
                    return Unauthorized();

                if(!available)
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
                UserDTO userDto = service.GetById(id);
                UserAndRols userAndRols = new UserAndRols
                {
                    UserId = userDto.UserId,
                    UserName = userDto.UserName,
                    Email = userDto.Email,
                    Roles =  userRolService.GetRolsByUserId(id)
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
                if(service.CheckUserName(data.UserName))
                    return BadRequest("User name already exists");
                if (!data.Email.ValidateEmail())
                    return BadRequest("Invalid email");
                if (!data.Password.ValidatePassword())
                    return BadRequest("Invalid password, use lowercase, uppercase, numbers and symbols, min length 8.");

                return Ok(service.Insert(data));
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

                if (service.CheckUserName(data.UserName, data.UserId))
                    return BadRequest("User name already exists");
                if (!data.Email.ValidateEmail())
                    return BadRequest("Invalid email");
                if (!TripleDESHelper.Decrypt(data.Password).ValidatePassword())
                    return BadRequest("Invalid password, use lowercase, uppercase, numbers and symbols, min length 8.");

                return Ok(service.Update(data));
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
                if (!TripleDESHelper.Decrypt(data.NewPassword).ValidatePassword())
                    return BadRequest("Invalid password, use lowercase, uppercase, numbers and symbols, min length 8.");
                if(!service.ValidPassword(new UserLogin { Username = data.UserName, Password = data.OldPassword }))
                    return BadRequest("Invalid password");
                return Ok(service.ChangePassword(data));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete, Route("{id}")]
        public ActionResult<UserDTO> Delete(int id)
        {
            try
            {
                return Ok(service.Delete(id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
