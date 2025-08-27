using Dapper;
using GCatcode.Repository.DB.UserRolServices;
using GCatcode.Utils;
using GCatcode.Utils.extensions;
using Microsoft.Data.SqlClient;
using System.Data;

namespace GCatcode.Repository.DB.UserServices
{
    public class UserService : DBService, IDBService<UserDTO, UserInsert, UserUpdate>
    {
        public UserService(string connectionString)
            : base(connectionString)
        {
        }

        public UserService(SqlConnection dbConnection, IDbTransaction transaction) 
            : base(dbConnection, transaction)
        {
        }

        public UserService(SqlConnection dbConnection) 
            : base(dbConnection, null)
        {
        }

        public object ChangePassword(UserChangePassword data)
        {
            if(TripleDESHelper.Decrypt(data.NewPassword).ValidatePassword())
            {
                return new { message = "The new password does not meet the security requirements." };
            }
            if (TripleDESHelper.Decrypt(data.OldPassword) == TripleDESHelper.Decrypt(data.NewPassword))
            {
                return new { message = "The new password is the same as the current password" };
            }

            DbConnection.Execute(
                @"UPDATE [dbo].[Users]
                        SET Password = @Password,
                            LastUpdated = @dateTime
                        WHERE UserId = @UserId
                    ",
                new
                {
                    data.UserId,
                    Password = data.NewPassword,
                    dateTime = DateTime.UtcNow
                }, Transaction);
            return new { message = "Password changed successfully" };
        }

        public bool CheckUserName(string userName, int? userId = null)
        {
            UserDTO? user = DbConnection.QueryFirstOrDefault<UserDTO>(
                @"SELECT * FROM [dbo].[Users] WHERE UserName = @userName AND UserId != @userId", new { userName, userId }, Transaction);
            return user != null;
        }

        public UserDTO Delete(int id)
        {
            return DbConnection.QueryFirstOrDefault<UserDTO>(
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
                }, Transaction);
        }

        public IEnumerable<UserDTO> Get(int maxItems = 100, int page = 1, object filters = null)
        {
            return DbConnection.Query<UserDTO>(
                $@"SELECT TOP {maxItems} * FROM [dbo].[Users] WHERE 1 = 1 {SQLUtils.ParseWere(filters)}", filters, Transaction);
        }

        public UserDTO GetById(int id)
        {
            return DbConnection.QueryFirstOrDefault<UserDTO>(
                @"SELECT * FROM [dbo].[Users] WHERE UserId = @id", new { id }, Transaction);
        }

        public UserDTO GetByUserName(string userName)
        {
            return DbConnection.QueryFirstOrDefault<UserDTO>(
                @"SELECT * FROM [dbo].[Users] WHERE UserName = @userName AND Available = 1", new { userName }, Transaction);
        }

        public UserDTO Add(UserInsert user)
        {
            ValidUser(user);

            var res = DbConnection.QueryFirstOrDefault<UserDTO>(
                @"INSERT INTO [dbo].[Users] (UserName, Email, Password) VALUES (@UserName, @Email, @Password)
                SELECT * FROM [dbo].[Users] WHERE UserId = @@IDENTITY", new { user.UserName, user.Email, user.Password }, Transaction);

            if (res == null)
            {
                throw new Exception("Error inserting user.");
            }

            UserRolService userRolService = new UserRolService(DbConnection, Transaction);
            userRolService.Insert(new UserRolDTO
            {
                UserId = res.UserId,
                RolId = RolServices.RolesType.Guest,
            });
            return res;
        }

        public UserDTO Update(UserUpdate data)
        {
            if (data.UserId <= 0)
            {
                throw new ArgumentException("UserId must be greater than 0.");
            }

            ValidUser(data);

            return DbConnection.QueryFirstOrDefault<UserDTO>(
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
                }, Transaction);
        }

        public bool ValidPassword(UserLogin userLogin)
        {
            UserUpdate? user = DbConnection.QueryFirstOrDefault<UserUpdate>(
                @"SELECT * FROM [dbo].[Users] WHERE UserName = @Username", new { userLogin.Username }, Transaction);
            if (user == null)
            {
                return false;
            }

            return (TripleDESHelper.Decrypt(user.Password) == TripleDESHelper.DecryptBase64(userLogin.Password));
        }

        private void ValidUser(UserInsert user)
        {
            if (string.IsNullOrEmpty(user.UserName) || string.IsNullOrEmpty(user.Email) || string.IsNullOrEmpty(user.Password))
            {
                throw new ArgumentException("Username, email, and password are required.");
            }

            if (CheckUserName(user.UserName))
            {
                throw new ArgumentException("User already exists.");
            }

            if (!user.Email.ValidateEmail())
            {
                throw new ArgumentException("Invalid email format.");
            }

            if (!user.Password.ValidatePassword())
            {
                throw new ArgumentException("Password must contain at least 8 characters, including uppercase, lowercase, numbers, and symbols.");
            }
        }

        private void ValidUser(UserUpdate user)
        {
            if (string.IsNullOrEmpty(user.UserName) || string.IsNullOrEmpty(user.Email) || string.IsNullOrEmpty(user.Password))
            {
                throw new ArgumentException("Username, email, and password are required.");
            }

            if (CheckUserName(user.UserName, user.UserId))
            {
                throw new ArgumentException("User already exists.");
            }

            if (!user.Email.ValidateEmail())
            {
                throw new ArgumentException("Invalid email format.");
            }
        }
    }
}