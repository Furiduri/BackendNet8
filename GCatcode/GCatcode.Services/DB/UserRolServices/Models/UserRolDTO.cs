using GCatcode.Repository.DB.RolServices.Models;

namespace GCatcode.Repository.DB.UserRolServices.Models
{
    public class UserRolDTO
    {
        public int UserId { get; set; }
        public RolesType RolId { get; set; }
    }
}