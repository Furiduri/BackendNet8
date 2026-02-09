using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace GCatcode.DataBase.Models
{
    [Index(nameof(UserName), IsUnique = true)]
    [Index(nameof(Email), IsUnique = true)]
    public class TR_User : BaseModel
    {
        [Required, Key]
        public int UserId { get; set; }

        [Required, MaxLength(250)]
        public string UserName { get; set; }

        [MaxLength(250)]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        public IEnumerable<RL_UserRol> UserRoles { get; internal set; }
    }
}