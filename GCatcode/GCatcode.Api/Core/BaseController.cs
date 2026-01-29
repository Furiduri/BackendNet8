using GCatcode.Api.Configuration;
using GCatcode.Repository.DB.RolServices;
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

        protected bool IsAdmin => User.Claims.Any(c => c.Type == ClaimTypes.Role &&
            (c.Value == RolesType.Admin.ToString() || c.Value == RolesType.Developer.ToString()));

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
            if (UserId < 1)
            {
                var claim = User.FindFirst(ClaimTypes.NameIdentifier);
                UserId = claim != null ? int.Parse(claim.Value) : -1;
            }
            return UserId.Value;
        }

        [NonAction]
        protected IEnumerable<RolItem> GetRoles()
        {
            using (var context = new SqlConnection(_configuration.DB.DefaultConnection))
            {
                return new RolesService(context).GetRolesByUserId(GetUserId());
            }
        }

        [NonAction]
        protected void LogError(Exception ex)
        {
            // Implement logging logic here, e.g., write in database log
        }
    }
}