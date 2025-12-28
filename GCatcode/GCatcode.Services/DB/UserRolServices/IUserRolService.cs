using GCatcode.Repository.DB.RolServices;
using GCatcode.Repository.DB.UserRolServices;

namespace GCatcode.Repository.DB.UserRolServices
{
    public interface IUserRolService
    {
        UserRolDTO Delete(int UserId, int RolId);
        IEnumerable<RolItem> GetRolsByUserId(int userId);
        UserRolDTO Insert(UserRolDTO data);
    }
}
