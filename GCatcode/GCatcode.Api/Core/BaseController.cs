using GCatcode.Repository.DB.RolServices;
using GCatcode.Repository.DB.UserRolServices;
using GCatcode.Repository.DB.UserServices;
using GCatcode.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Security.Claims;

namespace GCatcode.Api.Core
{
    public class BaseController : ControllerBase
    {
        public class Response<T>
        {
            public T Data { get; set; }
            public string Msg { get; set; }

            /// <summary>
            /// 0 success | 1 error | 5000 failed | HTTP code
            /// </summary>
            public int Error { get; set; } = 0;
        }

        protected readonly IConfiguration _configuration;
        protected readonly IUserService _userService;
        protected readonly IUserRolService _userRolService;

        public BaseController(IConfiguration configuration, IUserService userService, IUserRolService userRolService)
        {
            _configuration = configuration;
            _userService = userService;
            _userRolService = userRolService;
        }

        protected bool IsAdmin => User.Claims.Any(c => c.Type == ClaimTypes.Role && 
            (c.Value == RolesType.Admin.ToString() || c.Value == RolesType.Developer.ToString()));
        
        [NonAction]
        protected string GetUserName()
        {
            return User.FindFirst(ClaimTypes.Name)?.Value;
        }

        [NonAction]
        protected int GetUserId()
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier);
            return claim != null ? int.Parse(claim.Value) : -1;
        }

        [NonAction]
        protected List<RolItem> GetRoles()
        {
            return _userRolService.GetRolsByUserId(GetUserId()).ToList();
        }
    }
}