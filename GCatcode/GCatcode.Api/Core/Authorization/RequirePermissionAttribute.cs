using GCatcode.Api.Configuration;
using GCatcode.Services.DB.PermissionServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Data.SqlClient;
using System.Security.Claims;

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
            try
            {
                // Verificar si el usuario está autenticado
                if (!context.HttpContext.User.Identity?.IsAuthenticated ?? true)
                {
                    context.Result = new UnauthorizedObjectResult(AuthorizationResponse.Unauthorized());
                    return;
                }

                // Obtener UserId del token JWT
                var userIdClaim = context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
                {
                    context.Result = new UnauthorizedObjectResult(AuthorizationResponse.InvalidCredentials());
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
                        context.Result = new UnauthorizedObjectResult(AuthorizationResponse.Forbidden(_permissionCode));
                    }
                }

            }
            catch (Exception ex)
            {
                // Implement logging logic here, e.g., write in database log
                context.Result = new ObjectResult(AuthorizationResponse.InternalServerError(ex.Message));
            }
        }
    }
}