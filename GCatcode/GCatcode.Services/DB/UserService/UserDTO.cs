using GCatcode.Repository.DB.RolService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCatcode.Repository.DB.UserService
{
    public class UserDTO
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public IEnumerable<RolDTO> UserRoles { get; set; } = new List<RolDTO>();
    }
}
