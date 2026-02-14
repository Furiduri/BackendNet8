using GCatcode.Api.Configuration;
using GCatcode.Api.Core;
using GCatcode.Api.Core.Authorization;
using GCatcode.Repository.DB.PermissionServices.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GCatcode.Api.Controllers.Permissions
{
    [ApiController, Route("api/[controller]")]
    public class PermissionsController : BaseController
    {
        private readonly PermissionMethods _methods;

        public PermissionsController(AppSettings configuration) : base(configuration)
        {
            _methods = new PermissionMethods(configuration);
        }

        /// <summary>
        /// Obtiene todos los permisos disponibles en el sistema
        /// </summary>
        [HttpGet, Authorize]
        [RequirePermission("permissions.read")]
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
                var response = PermissionResponse.InternalServerError(ex.Message);
                return StatusCode((int)response.StatusCode, response);
            }
        }

        /// <summary>
        /// Obtiene un permiso por su ID
        /// </summary>
        [HttpGet("{id}"), Authorize]
        [RequirePermission("permissions.read")]
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
                var response = PermissionResponse.InternalServerError(ex.Message);
                return StatusCode((int)response.StatusCode, response);
            }
        }

        /// <summary>
        /// Obtiene todos los permisos del usuario actual (RBAC + ABAC)
        /// </summary>
        [HttpGet("my-permissions"), Authorize]
        public IActionResult GetMyPermissions()
        {
            try
            {
                var response = _methods.GetUserPermissions(GetUserId());
                return StatusCode(response.StatusCode, response);
            }
            catch (Exception ex)
            {
                LogError(ex);
                var response = PermissionResponse.InternalServerError(ex.Message);
                return StatusCode((int)response.StatusCode, response);
            }
        }

        /// <summary>
        /// Obtiene todos los permisos de un usuario específico (RBAC + ABAC)
        /// </summary>
        [HttpGet("user/{userId}"), Authorize]
        [RequirePermission("permissions.read")]
        public IActionResult GetUserPermissions(int userId)
        {
            try
            {
                var response = _methods.GetUserPermissions(userId);
                return StatusCode(response.StatusCode, response);
            }
            catch (Exception ex)
            {
                LogError(ex);
                var response = PermissionResponse.InternalServerError(ex.Message);
                return StatusCode((int)response.StatusCode, response);
            }
        }

        /// <summary>
        /// Obtiene todos los permisos de un rol
        /// </summary>
        [HttpGet("role/{roleId}"), Authorize]
        [RequirePermission("permissions.read")]
        public IActionResult GetRolePermissions(int roleId)
        {
            try
            {
                var response = _methods.GetRolePermissions(roleId);
                return StatusCode(response.StatusCode, response);
            }
            catch (Exception ex)
            {
                LogError(ex);
                var response = PermissionResponse.InternalServerError(ex.Message);
                return StatusCode((int)response.StatusCode, response);
            }
        }

        /// <summary>
        /// Asigna un permiso directo a un usuario (ABAC - Override)
        /// </summary>
        [HttpPost("user"), Authorize]
        [RequirePermission("permissions.write")]
        public IActionResult GrantPermissionToUser([FromBody] UserPermissionInsert data)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(PermissionResponse.InvalidData());

                var response = _methods.GrantPermissionToUser(data);
                return StatusCode(response.StatusCode, response);
            }
            catch (Exception ex)
            {
                LogError(ex);
                var response = PermissionResponse.InternalServerError(ex.Message);
                return StatusCode((int)response.StatusCode, response);
            }
        }

        /// <summary>
        /// Revoca un permiso directo de un usuario
        /// </summary>
        [HttpDelete("user/{userId}/{permissionId}"), Authorize]
        [RequirePermission("permissions.write")]
        public IActionResult RevokePermissionFromUser(int userId, int permissionId)
        {
            try
            {
                var response = _methods.RevokePermissionFromUser(userId, permissionId);
                return StatusCode(response.StatusCode, response);
            }
            catch (Exception ex)
            {
                LogError(ex);
                var response = PermissionResponse.InternalServerError(ex.Message);
                return StatusCode((int)response.StatusCode, response);
            }
        }

        /// <summary>
        /// Asigna un permiso a un rol (RBAC)
        /// </summary>
        [HttpPost("role"), Authorize]
        [RequirePermission("permissions.write")]
        public IActionResult GrantPermissionToRole([FromBody] RolePermissionInsert data)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(PermissionResponse.InvalidData());

                var response = _methods.GrantPermissionToRole(data);
                return StatusCode(response.StatusCode, response);
            }
            catch (Exception ex)
            {
                LogError(ex);
                var response = PermissionResponse.InternalServerError(ex.Message);
                return StatusCode((int)response.StatusCode, response);
            }
        }

        /// <summary>
        /// Revoca un permiso de un rol
        /// </summary>
        [HttpDelete("role/{roleId}/{permissionId}"), Authorize]
        [RequirePermission("permissions.write")]
        public IActionResult RevokePermissionFromRole(int roleId, int permissionId)
        {
            try
            {
                var response = _methods.RevokePermissionFromRole(roleId, permissionId);
                return StatusCode(response.StatusCode, response);
            }
            catch (Exception ex)
            {
                LogError(ex);
                var response = PermissionResponse.InternalServerError(ex.Message);
                return StatusCode((int)response.StatusCode, response);
            }
        }
    }
}
