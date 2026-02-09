using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GCatcode.DataBase.Models
{
    public class RL_ViewRol
    {
        [Required]
        public int RolId { get; set; }

        [Required]
        public int ViewId { get; set; }

        [ForeignKey(nameof(RolId))]
        public CL_Rol Rol { get; set; }

        [ForeignKey(nameof(ViewId))]
        public CL_View View { get; set; }
    }
}