using GCatcode.Api.Configuration;
using GCatcode.Api.Core;
using GCatcode.Api.Core.Authorization;
using GCatcode.Services.DB.ViewServices.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GCatcode.Api.Controllers.Views
{
    [ApiController, Route("api/[controller]")]
    public class ViewsController : BaseController
    {
        private readonly ViewMethods _methods;

        public ViewsController(AppSettings configuration) : base(configuration)
        {
            _methods = new ViewMethods(configuration);
        }

        /// <summary>
        /// Obtiene todas las vistas del sistema
        /// </summary>
        [HttpGet("all"), Authorize]
        [RequirePermission("views.read")]
        public IActionResult GetAll(int page = 1, int pageSize = 10)
        {
            try
            {
                var response = _methods.GetAll(page, pageSize);
                return StatusCode(response.StatusCode, response);
            }
            catch (Exception ex)
            {
                LogError(ex);
                var response = ViewResponse.InternalServerError(ex.Message);
                return StatusCode((int)response.StatusCode, response);
            }
        }

        /// <summary>
        /// Obtiene una vista por su ID
        /// </summary>
        [HttpGet("{id}"), Authorize]
        [RequirePermission("views.read")]
        public IActionResult GetById(int id)
        {
            try
            {
                var response = _methods.GetById(id);
                return StatusCode(response.StatusCode, response);
            }
            catch (Exception ex)
            {
                LogError(ex);
                var response = ViewResponse.InternalServerError(ex.Message);
                return StatusCode((int)response.StatusCode, response);
            }
        }

        /// <summary>
        /// Obtiene las vistas asignadas al usuario actual
        /// </summary>
        [HttpGet, Authorize]
        public IActionResult GetMyViews()
        {
            try
            {
                var response = _methods.GetByUserId(GetUserId());
                return StatusCode(response.StatusCode, response);
            }
            catch (Exception ex)
            {
                LogError(ex);
                var response = ViewResponse.InternalServerError(ex.Message);
                return StatusCode((int)response.StatusCode, response);
            }
        }

        /// <summary>
        /// Obtiene las vistas asignadas a un usuario específico
        /// </summary>
        [HttpGet("user/{userId}"), Authorize]
        [RequirePermission("views.read")]
        public IActionResult GetByUserId(int userId)
        {
            try
            {
                var response = _methods.GetByUserId(userId);
                return StatusCode(response.StatusCode, response);
            }
            catch (Exception ex)
            {
                LogError(ex);
                var response = ViewResponse.InternalServerError(ex.Message);
                return StatusCode((int)response.StatusCode, response);
            }
        }

        /// <summary>
        /// Obtiene las vistas asignadas a un rol
        /// </summary>
        [HttpGet("role/{roleId}"), Authorize]
        [RequirePermission("views.read")]
        public IActionResult GetByRoleId(int roleId)
        {
            try
            {
                var response = _methods.GetByRoleId(roleId);
                return StatusCode(response.StatusCode, response);
            }
            catch (Exception ex)
            {
                LogError(ex);
                var response = ViewResponse.InternalServerError(ex.Message);
                return StatusCode((int)response.StatusCode, response);
            }
        }

        /// <summary>
        /// Crea una nueva vista
        /// </summary>
        [HttpPost, Authorize]
        [RequirePermission("views.write")]
        public IActionResult Post([FromBody] ViewInsert data)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ViewResponse.InvalidData());

                var response = _methods.Post(data);
                return StatusCode(response.StatusCode, response);
            }
            catch (Exception ex)
            {
                LogError(ex);
                var response = ViewResponse.InternalServerError(ex.Message);
                return StatusCode((int)response.StatusCode, response);
            }
        }

        /// <summary>
        /// Actualiza una vista existente
        /// </summary>
        [HttpPut, Authorize]
        [RequirePermission("views.write")]
        public IActionResult Put([FromBody] ViewUpdate data)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ViewResponse.InvalidData());

                var response = _methods.Put(data);
                return StatusCode(response.StatusCode, response);
            }
            catch (Exception ex)
            {
                LogError(ex);
                var response = ViewResponse.InternalServerError(ex.Message);
                return StatusCode((int)response.StatusCode, response);
            }
        }

        /// <summary>
        /// Elimina una vista
        /// </summary>
        [HttpDelete("{id}"), Authorize]
        [RequirePermission("views.write")]
        public IActionResult Delete(int id)
        {
            try
            {
                var response = _methods.Delete(id);
                return StatusCode(response.StatusCode, response);
            }
            catch (Exception ex)
            {
                LogError(ex);
                var response = ViewResponse.InternalServerError(ex.Message);
                return StatusCode((int)response.StatusCode, response);
            }
        }

        /// <summary>
        /// Asigna un rol a una vista
        /// </summary>
        [HttpPost("roles"), Authorize]
        [RequirePermission("views.write")]
        public IActionResult AssignRoleToView([FromBody] ViewRoleAssignment viewToRole)
        {
            try
            {
                var response = _methods.AssignRoleToView(viewToRole);
                return StatusCode(response.StatusCode, response);
            }
            catch (Exception ex)
            {
                LogError(ex);
                var response = ViewResponse.InternalServerError(ex.Message);
                return StatusCode((int)response.StatusCode, response);
            }
        }

        /// <summary>
        /// Remueve un rol de una vista
        /// </summary>
        [HttpDelete("roles"), Authorize]
        [RequirePermission("views.write")]
        public IActionResult RemoveRoleFromView([FromBody] ViewRoleAssignment viewToRole)
        {
            try
            {
                var response = _methods.RemoveRoleFromView(viewToRole);
                return StatusCode(response.StatusCode, response);
            }
            catch (Exception ex)
            {
                LogError(ex);
                var response = ViewResponse.InternalServerError(ex.Message);
                return StatusCode((int)response.StatusCode, response);
            }
        }

        /// <summary>
        /// Asigna múltiples roles a una vista (reemplaza asignaciones existentes)
        /// </summary>
        [HttpPut("roles"), Authorize]
        [RequirePermission("views.write")]
        public IActionResult AssignRolesToView([FromBody] ViewAssignmentRoleList assignmentRoleList)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ViewResponse.InvalidData());

                var response = _methods.AssignRolesToView(assignmentRoleList);
                return StatusCode(response.StatusCode, response);
            }
            catch (Exception ex)
            {
                LogError(ex);
                var response = ViewResponse.InternalServerError(ex.Message);
                return StatusCode((int)response.StatusCode, response);
            }
        }
    }
}
