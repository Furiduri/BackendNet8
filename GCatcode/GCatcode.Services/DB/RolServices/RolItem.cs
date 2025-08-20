using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCatcode.Repository.DB.RolServices
{
    public class RolItem
    {
        public int RolId { get; set; }

        [Required, MaxLength(250)]
        public required string Name { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }
    }
}
