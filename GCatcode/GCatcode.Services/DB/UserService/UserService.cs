using Dapper;
using GCatcode.Utils;
using Microsoft.Data.SqlClient;
using System.Data;

namespace GCatcode.Repository.DB.UserService
{
    public class UserService : IDBService<UserDTO, UserInsert, UserUpdate>
    {
        public UserService(string connectionString)
        {
            ConnectionString = connectionString;
        }

        private readonly string ConnectionString;

        public UserDTO Delete(int id)
        {
            using var DbConnection = new SqlConnection(ConnectionString);
            return DbConnection.QueryFirst<UserDTO>(
                @"UPDATE [dbo].[Users]
                    SET Available = 0,
                        LastUpdated = @dateTime
                    WHERE UserId = @UserId
                    SELECT * FROM [dbo].[Users] WHERE UserId = @UserId
                ",
                new
                {
                    UserId = id,
                    dateTime = DateTime.UtcNow
                });
        }

        public IEnumerable<UserDTO> Get(int maxItems = 100, int page = 1, object filters = null)
        {
            using var DbConnection = new SqlConnection(ConnectionString);
            return DbConnection.Query<UserDTO>(
                $@"SELECT TOP {maxItems} * FROM [dbo].[Users] WHERE 1 = 1 {SQLUtils.ParseWere(filters)}", filters);
        }

        public UserDTO GetById(int id)
        {
            using var DbConnection = new SqlConnection(ConnectionString);
            return DbConnection.QueryFirst<UserDTO>(
                @"SELECT * FROM [dbo].[Users] WHERE UserId = @id", new { id });
        }

        public UserDTO Insert(UserInsert data)
        {
            using var DbConnection = new SqlConnection(ConnectionString);
            return DbConnection.QueryFirst<UserDTO>(
                @"INSERT INTO [dbo].[Users] (UserName, Email, Password) VALUES (@UserName, @Email, @Password)
                SELECT * FROM [dbo].[Users] WHERE UserId = @@IDENTITY", new { data.UserName, data.Email, data.Password });
        }

        public UserDTO Update(UserUpdate data)
        {
            using var DbConnection = new SqlConnection(ConnectionString);
            return DbConnection.QueryFirst<UserDTO>(
                $@"UPDATE [dbo].[Users]
                        SET UserName = @UserName,
                            Email = @Email,
                            Password = @Password,
                            Available = @Available,
                            LastUpdated = @dateTime
                        WHERE UserId = @UserId
                        SELECT * FROM [dbo].[Users] WHERE UserId = @UserId
                        ",
                new
                {
                    data.UserId,
                    data.UserName,
                    data.Email,
                    data.Password,
                    data.Available,
                    dateTime = DateTime.UtcNow
                });
        }

        public bool CheckUserName(string userName, int? userId = null)
        {
            using var DbConnection = new SqlConnection(ConnectionString);
            UserDTO? user = DbConnection.QueryFirstOrDefault<UserDTO>(
                @"SELECT * FROM [dbo].[Users] WHERE UserName = @userName AND UserId != @userId", new { userName, userId });
            return user != null;
        }

        public UserDTO GetUserByUserName(string userName)
        {
            using var DbConnection = new SqlConnection(ConnectionString);
            return DbConnection.QueryFirstOrDefault<UserDTO>(
                @"SELECT * FROM [dbo].[Users] WHERE UserName = @userName AND Available = 1", new { userName });
        }
        public bool ValidPassword(UserLogin userLogin)
        {
            using var DbConnection = new SqlConnection(ConnectionString);
            UserUpdate? user = DbConnection.QueryFirstOrDefault<UserUpdate>(
                @"SELECT * FROM [dbo].[Users] WHERE UserName = @Username", new { userLogin.Username });
            if (user == null)
            {
                return false;
            }

            return (TripleDESHelper.Decrypt( user.Password ) == TripleDESHelper.Decrypt( userLogin.Password ));            
        }

        public object ChangePassword(UserChangePassword data)
        {
            if (TripleDESHelper.Decrypt(data.OldPassword) == TripleDESHelper.Decrypt(data.NewPassword))
            {
                return new { message = "The new password is the same as the current password" };
            }

            using var DbConnection = new SqlConnection(ConnectionString);
                DbConnection.QueryFirstOrDefault<UserDTO>(
                    @"UPDATE [dbo].[Users]
                        SET Password = @Password,
                            LastUpdated = @dateTime
                        WHERE UserId = @UserId
                    ",
                    new
                    {
                        data.UserId,
                        Password =  data.NewPassword,
                        dateTime = DateTime.UtcNow
                    });
            return new { message = "Password changed successfully" };
        }
    }
}