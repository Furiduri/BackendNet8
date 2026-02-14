using System.ComponentModel.DataAnnotations;

namespace GCatcode.DataBase.Models
{
    /// <summary>
    /// Catálogo de permisos del sistema (ABAC)
    /// </summary>
    public class CL_Permission : BaseModel
    {
        [Key]
        public int PermissionId { get; set; }

        [Required, MaxLength(100)]
        public string Code { get; set; } = string.Empty; // Ej: "users.read", "users.write"

        [Required, MaxLength(100)]
        public string Resource { get; set; } = string.Empty; // Ej: "users", "roles", "reports"

        [Required, MaxLength(50)]
        public string Action { get; set; } = string.Empty; // Ej: "read", "write", "delete", "execute"

        [MaxLength(255)]
        public string Description { get; set; } = string.Empty;
    }
}
