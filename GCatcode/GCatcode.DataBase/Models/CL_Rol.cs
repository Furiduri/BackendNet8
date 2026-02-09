using System.ComponentModel.DataAnnotations;

namespace GCatcode.DataBase.Models
{
    public enum RolesType
    {
        Dev = 1,
        Guest = 2,
        User = 3,
        Admin = 4
    }

    public class CL_Rol : BaseModel
    {
        [Key]
        public int RolId { get; set; }

        [Required, MaxLength(250)]
        public string Name { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        public IEnumerable<RL_UserRol> UserRoles { get; internal set; }
        public IEnumerable<RL_ViewRol> ViewRols { get; internal set; }
    }
}