using GCatcode.Api.Configuration;
using GCatcode.Repository.DB.PermissionServices;
using GCatcode.Repository.DB.PermissionServices.Models;
using GCatcode.Repository.DB.RolServices.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Security.Claims;

namespace GCatcode.Api.Core
{
    public class BaseController : ControllerBase
    {
        protected readonly AppSettings _configuration;

        public BaseController(AppSettings configuration)
        {
            _configuration = configuration;
        }

        protected bool IsAdmin => GetRoles()?.Any(c => c == RolesType.Admin.ToString() || c == RolesType.Dev.ToString()) ?? false;

        private string? UserName { get; set; }

        [NonAction]
        protected string GetUserName()
        {
            if (string.IsNullOrEmpty(UserName))
            {
                UserName = User.FindFirstValue(ClaimTypes.Name);
            }
            return UserName ?? string.Empty;
        }

        private string? UserEmail { get; set; }

        [NonAction]
        protected string GetUserEmail()
        {
            if (string.IsNullOrEmpty(UserEmail))
            {
                UserEmail = User.FindFirstValue(ClaimTypes.Email);
            }
            return UserEmail ?? string.Empty;
        }

        private int? UserId { get; set; }

        [NonAction]
        protected int GetUserId()
        {
            if (UserId == null)
            {
                var claim = User.FindFirst(ClaimTypes.NameIdentifier);
                UserId = claim != null ? int.Parse(claim.Value) : -1;
            }
            return UserId.Value;
        }

        private IEnumerable<string>? Roles;

        [NonAction]
        protected IEnumerable<string> GetRoles()
        {
            if (Roles == null)
            {
                var claims = User.FindAll(ClaimTypes.Role);
                Roles = claims.Select(x => x.Value);
            }
            return Roles;
        }

        // ===== MÉTODOS PARA PERMISOS (RBAC + ABAC) =====

        private IEnumerable<UserPermissionResult>? _userPermissions;

        /// <summary>
        /// Obtiene todos los permisos del usuario actual (RBAC + ABAC)
        /// </summary>
        [NonAction]
        protected IEnumerable<UserPermissionResult> GetUserPermissions()
        {
            if (_userPermissions == null)
            {
                using (var connection = new SqlConnection(_configuration.DB.DefaultConnection))
                {
                    var permissionService = new PermissionService(connection);
                    _userPermissions = permissionService.GetUserPermissions(GetUserId()).ToList();
                }
            }
            return _userPermissions;
        }

        /// <summary>
        /// Verifica si el usuario tiene un permiso específico
        /// Prioriza permisos directos sobre heredados de roles
        /// </summary>
        [NonAction]
        protected bool HasPermission(string permissionCode)
        {
            using (var connection = new SqlConnection(_configuration.DB.DefaultConnection))
            {
                var permissionService = new PermissionService(connection);
                return permissionService.HasPermission(GetUserId(), permissionCode);
            }
        }

        /// <summary>
        /// Verifica si el usuario tiene todos los permisos especificados
        /// </summary>
        [NonAction]
        protected bool HasAllPermissions(params string[] permissionCodes)
        {
            return permissionCodes.All(code => HasPermission(code));
        }

        /// <summary>
        /// Verifica si el usuario tiene al menos uno de los permisos especificados
        /// </summary>
        [NonAction]
        protected bool HasAnyPermission(params string[] permissionCodes)
        {
            return permissionCodes.Any(code => HasPermission(code));
        }

        [NonAction]
        protected void LogError(Exception ex)
        {
            // Implement logging logic here, e.g., write in database log
        }
    }
}