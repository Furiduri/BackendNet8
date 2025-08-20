using GCatcode.Repository.DB.RolServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCatcode.Repository.DB.UserRolServices
{
    public class UserRolDTO
    {
        public int UserId { get; set; }
        public RolesType RolId { get; set; }
    }
}
