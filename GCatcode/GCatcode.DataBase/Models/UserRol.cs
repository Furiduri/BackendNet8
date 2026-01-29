using System.ComponentModel.DataAnnotations;

namespace GCatcode.DataBase.Models
{
    public class UserRol : BaseModel
    {
        public int UserId { get; set; }
        public int RolId { get; set; }
        public User User { get; set; }
        public Rol Rol { get; set; }
    }
}
