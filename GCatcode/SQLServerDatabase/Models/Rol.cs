using System.ComponentModel.DataAnnotations;

namespace GCatcode.SQLServerDatabase.Models
{
    public class Rol : BaseTable
    {
        [Key]
        public int RolId { get; set; }

        [Required, MaxLength(250)]
        public required string Name { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }
    }
}