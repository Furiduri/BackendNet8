using System.ComponentModel.DataAnnotations;

namespace GCatcode.DataBase.Models
{
    public enum RolesType
    {
        Dev = 0,
        Guest = 1,
        User = 2,
        Admin = 3
    }

    public class Rol : BaseModel
    {
        [Key]
        public int RolId { get; set; }

        [Required, MaxLength(250)]
        public string Name { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        public IEnumerable<UserRol> UserRoles { get; internal set; }
    }
}