using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GCatcode.SQLServerDatabase
{
    public class BaseModel
    {
        [Required, DefaultValue(true)]
        public bool Available { get; set; } = true;

        [Required]
        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;

        [Required]
        public DateTime CreateTime { get; set; } = DateTime.UtcNow;
    }
}