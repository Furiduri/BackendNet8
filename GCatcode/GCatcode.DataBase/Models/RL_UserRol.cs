using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GCatcode.DataBase.Models
{
    public class RL_UserRol : BaseModel
    {
        [Required]
        public int UserId { get; set; }
        
        [Required]
        public int RolId { get; set; }

        [ForeignKey(nameof(UserId))]
        public TR_User User { get; set; }

        [ForeignKey(nameof(RolId))]
        public CL_Rol Rol { get; set; }

    }
}