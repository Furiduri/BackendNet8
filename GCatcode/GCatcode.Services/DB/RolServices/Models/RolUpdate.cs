using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GCatcode.Services.DB.RolServices.Models
{
    public class RolUpdate
    {
        public int RolId { get; set; }

        [Required, MaxLength(250)]
        public string Name { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }

        [Required, DefaultValue(true)]
        public bool Available { get; set; }
    }
}