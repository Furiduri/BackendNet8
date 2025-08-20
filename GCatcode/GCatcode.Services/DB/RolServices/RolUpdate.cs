using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GCatcode.Repository.DB.RolServices
{
    public class RolUpdate
    {
        public int RolId { get; set; }

        [Required, MaxLength(250)]
        public required string Name { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }

        [Required, DefaultValue(true)]
        public bool Available { get; set; }
    }
}