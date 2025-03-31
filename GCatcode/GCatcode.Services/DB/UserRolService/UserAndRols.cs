using GCatcode.Repository.DB.RolService;
using GCatcode.Repository.DB.UserService;

namespace GCatcode.Repository.DB.UserRolService
{
    public class UserAndRols : UserDTO
    {
        public IEnumerable<RolItem> Roles { get; set; }
    }
}
