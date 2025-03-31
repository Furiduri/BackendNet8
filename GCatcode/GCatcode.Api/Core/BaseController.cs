using GCatcode.Repository.DB.RolService;
using GCatcode.Repository.DB.UserRolService;
using GCatcode.Utils;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GCatcode.Api.Core
{
    public class BaseController : ControllerBase
    {
        protected readonly IConfiguration _configuration;

        public BaseController(IConfiguration configuration)
        { _configuration = configuration; }

        protected bool IsAdmin => GetRoles().Exists(x => x.RolId == (int)RolesType.Admin || x.RolId == (int)RolesType.Developer);
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

        protected List<RolItem> GetRoles()
        {
            try
            {
                var userRolService = new UserRolService(Settings.GetBaseDBConnection(_configuration));

                return userRolService.GetRolsByUserId(GetUserId()).ToList();
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}