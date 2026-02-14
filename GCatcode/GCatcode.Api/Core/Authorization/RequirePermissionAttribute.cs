using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;
using GCatcode.Repository.DB.PermissionServices;
using Microsoft.Data.SqlClient;
using GCatcode.Api.Configuration;

namespace GCatcode.Api.Core.Authorization
{
    /// <summary>
    /// Atributo para validar permisos en endpoints (RBAC + ABAC)
    /// Uso: [RequirePermission("users.write")]
    /// Los permisos directos de usuario tienen prioridad sobre los heredados de roles
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
    public class RequirePermissionAttribute : Attribute, IAuthorizationFilter
    {
        private readonly string _permissionCode;

        public RequirePermissionAttribute(string permissionCode)
        {
            _permissionCode = permissionCode;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            // Verificar si el usuario está autenticado
            if (!context.HttpContext.User.Identity?.IsAuthenticated ?? true)
            {
                context.Result = new UnauthorizedObjectResult(new
                {
                    StatusCode = 401,
                    Message = "Usuario no autenticado"
                });
                return;
            }

            // Obtener UserId del token JWT
            var userIdClaim = context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                context.Result = new UnauthorizedObjectResult(new
                {
                    StatusCode = 401,
                    Message = "Token inválido"
                });
                return;
            }

            // Obtener configuración de la aplicación
            var configuration = context.HttpContext.RequestServices.GetRequiredService<AppSettings>();

            // Verificar permiso
            using (var connection = new SqlConnection(configuration.DB.DefaultConnection))
            {
                var permissionService = new PermissionService(connection);
                bool hasPermission = permissionService.HasPermission(userId, _permissionCode);

                if (!hasPermission)
                {
                    context.Result = new ObjectResult(new
                    {
                        StatusCode = 403,
                        Message = $"Acceso denegado. Se requiere el permiso: {_permissionCode}"
                    })
                    {
                        StatusCode = 403
                    };
                }
            }
        }
    }
}
