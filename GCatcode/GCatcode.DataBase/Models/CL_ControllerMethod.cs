using System.ComponentModel.DataAnnotations;

namespace GCatcode.DataBase.Models
{
    public class CL_ControllerMethod : BaseModel
    {
        [Key]
        public int MethodId { get; set; }

        [Required]
        public int ControllerId { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; }

        [Required, MaxLength(100)]
        public string HttpMethod { get; set; }

        [Required, MaxLength(200)]
        public string Endpoint { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        [Required]
        public PermissionType PermissionType { get; set; }
    }

    public enum PermissionType
    {
        Read = 1,      // Solo lectura (GET)
        Write = 2,     // Solo escritura (POST, PUT)
        ReadWrite = 3  // Lectura y escritura (GET, POST, PUT, DELETE)
    }
}