using GCatcode.Repository.DB.UserServices;

namespace GCatcode.Repository.DB.UserServices
{
    public interface IUserService : IDBService<UserDTO, UserInsert, UserUpdate>
    {
        object ChangePassword(UserChangePassword data);
        bool CheckUserName(string userName, int? userId = null);
        UserDTO GetByUserName(string userName);
        bool ValidPassword(UserLogin userLogin);
        UserUpdate GetUpdateById(int id);
    }
}
