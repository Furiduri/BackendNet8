using Dapper;
using GCatcode.Repository.DB.UserRolServices;
using GCatcode.Utils;
using GCatcode.Utils.extensions;
using Microsoft.Data.SqlClient;
using System.Data;

namespace GCatcode.Repository.DB.UserServices
{
    public class UserService : DBService
    {
        public UserService(SqlConnection dbConnection)
            : base(dbConnection, null)
        {
        }

        public UserService(SqlConnection dbConnection, IDbTransaction transaction)
            : base(dbConnection, transaction)
        {
        }

        public object ChangePassword(UserChangePassword data)
        {
            data.OldPassword = DecryptToBase64(data.OldPassword);
            data.NewPassword = DecryptToBase64(data.NewPassword);

            var user = GetUpdateById(data.UserId);
            if (user == null)
            {
                return new { message = "User not found." };
            }

            if (!Argon2Helper.VerifyPassword(data.OldPassword, user.Password))
            {
                return new { message = "Invalid current password." };
            }

            if (data.OldPassword == data.NewPassword)
            {
                return new { message = "The new password is the same as the current password." };
            }

            if (!data.NewPassword.ValidatePassword())
            {
                return new { message = "The new password does not meet the security requirements." };
            }

            string hashedNewPassword = Argon2Helper.HashPassword(data.NewPassword);

            DbConnection.Execute(
                @"UPDATE [dbo].[Users]
                        SET Password = @Password,
                            LastUpdated = @dateTime
                        WHERE UserId = @UserId
                    ",
                new
                {
                    data.UserId,
                    Password = hashedNewPassword,
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

        public IEnumerable<UserDTO> Get(int maxItems = 100, int page = 1, UserFilter filters = null)
        {
            return DbConnection.Query<UserDTO>(
                $@"SELECT * FROM [dbo].[Users] WHERE 1 = 1 {SQLUtils.ParseWere(filters)}
                    Order By UserId OFFSET {(page - 1) * maxItems} ROWS FETCH NEXT {maxItems} ROWS ONLY",
                filters, Transaction);
        }

        public UserDTO GetById(int id)
        {
            return DbConnection.QueryFirstOrDefault<UserDTO>(
                @"SELECT * FROM [dbo].[Users] WHERE UserId = @id", new { id }, Transaction);
        }

        public UserUpdate GetUpdateById(int id)
        {
            return DbConnection.QueryFirstOrDefault<UserUpdate>(
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
            user.Password = DecryptToBase64(user.Password);
            string hashedPassword = Argon2Helper.HashPassword(user.Password);

            var res = DbConnection.QueryFirstOrDefault<UserDTO>(
                @"INSERT INTO [dbo].[Users] (UserName, Email, Password) VALUES (@UserName, @Email, @Password)
                SELECT * FROM [dbo].[Users] WHERE UserId = @@IDENTITY", new { user.UserName, user.Email, Password = hashedPassword }, Transaction);

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

            // Note: If updating password here, it should be hashed.
            // However, typically Update shouldn't change password unless explicitly handled.
            // For now, I'll keep the current logic but ensure it's hashed if changed.
            // But since Password passed in UserUpdate might be the hash already or a new plain password...
            // Decalring that for regular updates, we don't change password unless handled.
            // Looking at original code, it was just saving whatever was in data.Password.

            return DbConnection.QueryFirstOrDefault<UserDTO>(
                $@"UPDATE [dbo].[Users]
                        SET UserName = @UserName,
                            Email = @Email,
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
                    data.Available,
                    dateTime = DateTime.UtcNow
                }, Transaction);
        }

        public bool ValidPassword(UserLogin userLogin)
        {
            userLogin.Password = DecryptToBase64(userLogin.Password);
            var userByName = GetByUserName(userLogin.Username);
            if (userByName == null)
            {
                return false;
            }
            var user = GetUpdateById(userByName.UserId);

            return Argon2Helper.VerifyPassword(userLogin.Password, user.Password);
        }

        private string DecryptToBase64(string password)
        {
            //En caso de que el password no este en base 64, se retorna tal cual
            try
            {
                byte[] data = Convert.FromBase64String(password);
                return System.Text.Encoding.UTF8.GetString(data);
            }
            catch
            {
                return password;
            }
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
            if (string.IsNullOrEmpty(user.UserName) || string.IsNullOrEmpty(user.Email))
            {
                throw new ArgumentException("Username and email are required.");
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

        public IEnumerable<UserItem> Search(string userName, int page)
        {
            return DbConnection.Query<UserItem>(
                $@"SELECT UserId, UserName FROM [dbo].[Users]
                    WHERE UserName LIKE @userName AND Available = 1
                    ORDER BY UserName
                    OFFSET {(page - 1) * 10} ROWS FETCH NEXT 10 ROWS ONLY",
                new { userName = $"%{userName}%" }, Transaction);
        }
    }
}