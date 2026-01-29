using GCatcode.Api.Configuration;
using GCatcode.Repository.DB.RolServices;
using Microsoft.AspNetCore.Mvc;
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

        private string UserName { get; set; }

        [NonAction]
        protected string GetUserName()
        {
            if (string.IsNullOrEmpty(UserName))
            {
                UserName = User.FindFirstValue(ClaimTypes.Name);
            }
            return UserName;
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

        private IEnumerable<string> Roles;

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

        [NonAction]
        protected void LogError(Exception ex)
        {
            // Implement logging logic here, e.g., write in database log
        }
    }
}