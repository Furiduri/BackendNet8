using GCatcode.Api.Configuration;
using GCatcode.Api.Core;
using GCatcode.Repository.DB.UserRolServices;
using GCatcode.Repository.DB.UserServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GCatcode.Api.Controllers.Users
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : BaseController
    {
        private readonly UsersMethods _methods;

        public UsersController(AppSettings configuration)
            : base(configuration)
        {
            _methods = new UsersMethods(configuration);
        }

        [HttpGet("Search")]
        public ActionResult Search(string term, int page = 1)
        {
            try
            {
                ApiResponse response = _methods.SearchUsers(term, page);
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

        [HttpGet]
        public ActionResult<List<UserDTO>> Get(int page = 1, bool available = true)
        {
            try
            {
                if (!IsAdmin)
                    return Unauthorized();

                ApiResponse response = _methods.GetListUsers(page, available);
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
        public ActionResult<UserAndRols> GetById(int id)
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

        [HttpPost]
        public ActionResult<UserDTO> Post([FromBody] UserInsert data)
        {
            try
            {
                ApiResponse response = _methods.CreateUser(data);
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

        [HttpPut]
        public ActionResult<UserDTO> Put([FromBody] UserUpdate data)
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

        [HttpPut, Route("ChangePassword")]
        public ActionResult<UserDTO> ChangePassword([FromBody] UserChangePassword data)
        {
            try
            {
                ApiResponse response = _methods.ChangePassword(data);
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
        public ActionResult<UserDTO> Delete(int id)
        {
            if (!IsAdmin)
                return Unauthorized();
            if (id == GetUserId())
                return BadRequest(UserResponseMenssage.UserNotDeleteSelf.ToMsgString());

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