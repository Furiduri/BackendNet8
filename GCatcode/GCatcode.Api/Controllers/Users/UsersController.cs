using GCatcode.Api.Configuration;
using GCatcode.Api.Core;
using GCatcode.Repository.DB.UserRolServices.Models;
using GCatcode.Repository.DB.UserServices;
using GCatcode.Repository.DB.UserServices.Models;
using GCatcode.Utils.GenericModels;
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
        [ProducesResponseType<IEnumerable<UserItem>>(StatusCodes.Status200OK)]
        public ActionResult Search(string term, int page = 1)
        {
            try
            {
                var response = _methods.SearchUsers(term, page);
                return StatusCode(response.StatusCode, response);
            }
            catch (Exception ex)
            {
                LogError(ex);
                var response = ApiResponse<string>.ErrorResult(UserResponse.InternalServerError(ex.Message));
                return StatusCode((int)response.StatusCode, response);
            }
        }

        [HttpGet]
        [ProducesResponseType<IEnumerable<UserDTO>>(StatusCodes.Status200OK)]
        public ActionResult Get(int page = 1, bool available = true)
        {
            try
            {
                if (!IsAdmin)
                {
                    var status = ApiResponse<string>.ErrorResult(UserResponse.UserNotAdmin());
                    return StatusCode((int)status.StatusCode, status);
                }

                var response = _methods.GetListUsers(page, available);
                return StatusCode(response.StatusCode, response);
            }
            catch (Exception ex)
            {
                LogError(ex);
                var response = ApiResponse<string>.ErrorResult(UserResponse.InternalServerError(ex.Message));
                return StatusCode((int)response.StatusCode, response);
            }
        }

        [HttpGet, Route("{id}")]
        [ProducesResponseType<UserAndRols>(StatusCodes.Status200OK)]
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
                var response = ApiResponse<string>.ErrorResult(UserResponse.InternalServerError(ex.Message));
                return StatusCode((int)response.StatusCode, response);
            }
        }

        [HttpPost]
        [ProducesResponseType<UserDTO>(StatusCodes.Status200OK)]
        public ActionResult Post([FromBody] UserInsert data)
        {
            try
            {
                var response = _methods.CreateUser(data);
                return StatusCode(response.StatusCode, response);
            }
            catch (Exception ex)
            {
                LogError(ex);
                var response = ApiResponse<string>.ErrorResult(UserResponse.InternalServerError(ex.Message));
                return StatusCode((int)response.StatusCode, response);
            }
        }

        [HttpPut]
        [ProducesResponseType<UserDTO>(StatusCodes.Status200OK)]
        public ActionResult<UserDTO> Put([FromBody] UserUpdate data)
        {
            try
            {
                if (!IsAdmin)
                {
                    var status = ApiResponse<string>.ErrorResult(UserResponse.UserNotAdmin());
                    return StatusCode((int)status.StatusCode, status);
                }
                var response = _methods.Update(data);
                return StatusCode(response.StatusCode, response);
            }
            catch (Exception ex)
            {
                LogError(ex);
                var response = ApiResponse<string>.ErrorResult(UserResponse.InternalServerError(ex.Message));
                return StatusCode((int)response.StatusCode, response);
            }
        }

        [HttpPut, Route("ChangePassword")]
        [ProducesResponseType<GenericMessage>(StatusCodes.Status200OK)]
        public ActionResult ChangePassword([FromBody] UserChangePassword data)
        {
            try
            {
                var response = _methods.ChangePassword(data);
                return StatusCode(response.StatusCode, response);
            }
            catch (Exception ex)
            {
                LogError(ex);
                var response = ApiResponse<string>.ErrorResult(UserResponse.InternalServerError(ex.Message));
                return StatusCode((int)response.StatusCode, response);
            }
        }

        [HttpDelete, Route("{id}")]
        [ProducesResponseType<UserDTO>(StatusCodes.Status200OK)]
        public ActionResult Delete(int id)
        {
            if (!IsAdmin)
            {
                var status = ApiResponse<string>.ErrorResult(UserResponse.UserNotAdmin());
                return StatusCode((int)status.StatusCode, status);
            }

            if (id == GetUserId())
            {
                var status = ApiResponse<string>.ErrorResult(UserResponse.UserNotDeleteSelf());
                return StatusCode((int)status.StatusCode, status);
            }

            try
            {
                var response = _methods.Delete(id);
                return StatusCode(response.StatusCode, response);
            }
            catch (Exception ex)
            {
                LogError(ex);
                var response = ApiResponse<string>.ErrorResult(UserResponse.InternalServerError(ex.Message));
                return StatusCode((int)response.StatusCode, response);
            }
        }
    }
}