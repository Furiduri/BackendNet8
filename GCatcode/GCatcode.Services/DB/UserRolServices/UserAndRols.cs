using GCatcode.Repository.DB.RolServices;
using GCatcode.Repository.DB.UserServices;

namespace GCatcode.Repository.DB.UserRolServices
{
    public class UserAndRols : UserDTO
    {
        public IEnumerable<RolItem> Roles { get; set; }
    }
}
