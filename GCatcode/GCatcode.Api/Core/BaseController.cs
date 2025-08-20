using GCatcode.Repository.DB.RolServices;
using GCatcode.Repository.DB.UserRolServices;
using GCatcode.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Security.Claims;

namespace GCatcode.Api.Core
{
    public class BaseController : ControllerBase
    {
        protected readonly IConfiguration _configuration;
        protected readonly SqlConnection _connection;
        public BaseController(IConfiguration configuration)
        { _configuration = configuration;
          _connection = Settings.GetSqlDBConnection(_configuration);
        }

        protected bool IsAdmin => GetRoles().Exists(x => x.RolId == (int)RolesType.Admin || x.RolId == (int)RolesType.Developer);
        
        [NonAction]
        protected string GetUserName()
        {
            try
            {
                return User.FindFirst(ClaimTypes.Name).Value;
            }
            catch (Exception)
            {
                return null;
            }
        }

        [NonAction]
        protected int GetUserId()
        {
            try
            {
                return int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            }
            catch (Exception)
            {
                return -1;
            }
        }

        [NonAction]
        protected List<RolItem> GetRoles()
        {
            try
            {
                var userRolService = new UserRolService(_connection);
                return userRolService.GetRolsByUserId(GetUserId()).ToList();
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}