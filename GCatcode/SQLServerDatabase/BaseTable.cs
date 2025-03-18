using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GCatcode.SQLServerDatabase
{
    public class BaseTable
    {
        [Required, DefaultValue(true)]
        public bool Available { get; set; }

        [Required]
        public DateTime LastUpdated { get; set; }

        [Required]
        public DateTime CreateTime { get; set; }
    }
}