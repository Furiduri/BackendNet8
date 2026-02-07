using GCatcode.Repository.DB.RolServices.Models;
using GCatcode.Repository.DB.UserServices;

namespace GCatcode.Repository.DB.UserRolServices.Models
{
    public class UserAndRols : UserDTO
    {
        public IEnumerable<RolItem> Roles { get; set; }
    }
}