using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GCatcode.DataBase.Models
{
    /// <summary>
    /// Permisos específicos asignados directamente a usuarios (ABAC - Overrides)
    /// Los permisos de usuario tienen prioridad sobre los permisos heredados de roles
    /// </summary>
    public class RL_UserPermission : BaseModel
    {
        [Key]
        public int UserPermissionId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int PermissionId { get; set; }

        [Required]
        public bool IsGranted { get; set; } = true; // true = conceder, false = denegar

        [MaxLength(500)]
        public string Conditions { get; set; } // JSON con condiciones ABAC adicionales (ej: horarios, IPs)

        [ForeignKey(nameof(UserId))]
        public virtual TR_User User { get; set; }

        [ForeignKey(nameof(PermissionId))]
        public virtual CL_Permission Permission { get; set; }
    }
}
