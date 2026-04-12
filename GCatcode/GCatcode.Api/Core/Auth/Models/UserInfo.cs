using GCatcode.Services.DB.RolServices.Models;

namespace GCatcode.Api.Core.Auth.Models
{
    public class UserInfo
    {
        public string Email;
        public string UserName;
        public int UserId;
        public IEnumerable<RolItem> Roles;
    }
}