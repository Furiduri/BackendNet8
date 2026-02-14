using GCatcode.Api.Configuration;
using GCatcode.Api.Core;
using GCatcode.Api.Core.Authorization;
using GCatcode.Repository.DB.ViewServices.Models;
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
        [HttpGet, Authorize]
        [RequirePermission("views.read")]
        public IActionResult GetAll()
        {
            try
            {
                var response = _methods.GetAll();
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
        /// Obtiene todas las vistas con sus roles asignados
        /// </summary>
        [HttpGet("with-roles"), Authorize]
        [RequirePermission("views.read")]
        public IActionResult GetAllWithRoles()
        {
            try
            {
                var response = _methods.GetAllWithRoles();
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
        [HttpGet("my-views"), Authorize]
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
        [HttpPost("{viewId}/roles/{roleId}"), Authorize]
        [RequirePermission("views.write")]
        public IActionResult AssignRoleToView(int viewId, int roleId)
        {
            try
            {
                var response = _methods.AssignRoleToView(viewId, roleId);
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
        [HttpDelete("{viewId}/roles/{roleId}"), Authorize]
        [RequirePermission("views.write")]
        public IActionResult RemoveRoleFromView(int viewId, int roleId)
        {
            try
            {
                var response = _methods.RemoveRoleFromView(viewId, roleId);
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
        [HttpPut("{viewId}/roles"), Authorize]
        [RequirePermission("views.write")]
        public IActionResult AssignRolesToView(int viewId, [FromBody] IEnumerable<int> roleIds)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ViewResponse.InvalidData());

                var response = _methods.AssignRolesToView(viewId, roleIds);
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
