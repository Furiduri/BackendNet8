using System.ComponentModel.DataAnnotations;

namespace GCatcode.Services.DB.RolServices.Models
{
    public class RolItem
    {
        public int RolId { get; set; }

        [Required, MaxLength(250)]
        public string Name { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }
    }
}