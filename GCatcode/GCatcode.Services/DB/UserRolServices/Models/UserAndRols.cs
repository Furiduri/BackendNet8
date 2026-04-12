using GCatcode.Services.DB.RolServices.Models;
using GCatcode.Services.DB.UserServices;

namespace GCatcode.Services.DB.UserRolServices.Models
{
    public class UserAndRols : UserDTO
    {
        public IEnumerable<RolItem> Roles { get; set; }
    }
}