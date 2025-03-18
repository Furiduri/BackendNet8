using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GCatcode.Repository.DB.RolService
{
    public class RolDTO
    {
        public int RolId { get; set; }

        [Required, MaxLength(250)]
        public string Name { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }

        [Required, DefaultValue(true)]
        public bool Available { get; set; }

        [Required]
        public DateTime LastUpdated { get; set; }
    }
}