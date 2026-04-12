using GCatcode.Services.DB.RolServices.Models;

namespace GCatcode.Services.DB.UserRolServices.Models
{
    public class UserRolDTO
    {
        public int UserId { get; set; }
        public RolesType RolId { get; set; }
        public bool Available { get; set; }
    }
}