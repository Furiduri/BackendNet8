using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GCatcode.DataBase.Models
{
    /// <summary>
    /// Relación de permisos asignados a roles (RBAC)
    /// </summary>
    public class RL_RolePermission : BaseModel
    {
        [Key]
        public int RolePermissionId { get; set; }

        [Required]
        public int RolId { get; set; }

        [Required]
        public int PermissionId { get; set; }

        [ForeignKey(nameof(RolId))]
        public virtual CL_Rol? Role { get; set; }

        [ForeignKey(nameof(PermissionId))]
        public virtual CL_Permission? Permission { get; set; }
    }
}
