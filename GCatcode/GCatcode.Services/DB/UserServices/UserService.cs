using Dapper;
using GCatcode.Repository.DB.RolServices.Models;
using GCatcode.Repository.DB.UserRolServices;
using GCatcode.Repository.DB.UserRolServices.Models;
using GCatcode.Repository.DB.UserServices.Models;
using GCatcode.Utils;
using GCatcode.Utils.extensions;
using GCatcode.Utils.GenericModels;
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

        public GenericMessage ChangePassword(UserChangePassword data)
        {
            data.OldPassword = DecryptToBase64(data.OldPassword);
            data.NewPassword = DecryptToBase64(data.NewPassword);

            var user = GetUpdateById(data.UserId);
            if (user == null)
            {
                return new GenericMessage { Message = "User not found." };
            }

            if (!Argon2Helper.VerifyPassword(data.OldPassword, user.Password))
            {
                return new GenericMessage { Message = "Invalid current password." };
            }

            if (data.OldPassword == data.NewPassword)
            {
                return new GenericMessage { Message = "The new password is the same as the current password." };
            }

            if (!data.NewPassword.ValidatePassword())
            {
                return new GenericMessage { Message = "The new password does not meet the security requirements." };
            }

            string hashedNewPassword = Argon2Helper.HashPassword(data.NewPassword);

            DbConnection.Execute(
                @"UPDATE [dbo].[TR_Users]
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

            return new GenericMessage { Message = "Password changed successfully" };
        }

        public bool CheckUserName(string userName, int? userId = null)
        {
            UserDTO? user = DbConnection.QueryFirstOrDefault<UserDTO>(
                @"SELECT * FROM [dbo].[TR_Users] WHERE UserName = @userName AND UserId != @userId", new { userName, userId }, Transaction);
            return user != null;
        }

        public UserDTO Delete(int id)
        {
            return DbConnection.QueryFirstOrDefault<UserDTO>(
                @"UPDATE [dbo].[TR_Users]
                    SET Available = 0,
                        LastUpdated = @dateTime
                    WHERE UserId = @UserId
                    SELECT * FROM [dbo].[TR_Users] WHERE UserId = @UserId
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
                $@"SELECT * FROM [dbo].[TR_Users] WHERE 1 = 1 {SQLUtils.ParseWere(filters)}
                    Order By UserId OFFSET {(page - 1) * maxItems} ROWS FETCH NEXT {maxItems} ROWS ONLY",
                filters, Transaction);
        }

        public UserDTO GetById(int id)
        {
            return DbConnection.QueryFirstOrDefault<UserDTO>(
                @"SELECT * FROM [dbo].[TR_Users] WHERE UserId = @id", new { id }, Transaction);
        }

        public UserUpdate GetUpdateById(int id)
        {
            return DbConnection.QueryFirstOrDefault<UserUpdate>(
                @"SELECT * FROM [dbo].[TR_Users] WHERE UserId = @id", new { id }, Transaction);
        }

        public UserDTO GetByUserName(string userName)
        {
            return DbConnection.QueryFirstOrDefault<UserDTO>(
                @"SELECT * FROM [dbo].[TR_Users] WHERE UserName = @userName AND Available = 1", new { userName }, Transaction);
        }

        public UserDTO Add(UserInsert user)
        {
            user.Password = DecryptToBase64(user.Password);
            ValidUser(user);
            string hashedPassword = Argon2Helper.HashPassword(user.Password);

            var res = DbConnection.QueryFirstOrDefault<UserDTO>(
                @"INSERT INTO [dbo].[TR_Users] (UserName, Email, Password) VALUES (@UserName, @Email, @Password)
                SELECT * FROM [dbo].[TR_Users] WHERE UserId = @@IDENTITY", new { user.UserName, Email = user.Email.ToLower(), Password = hashedPassword }, Transaction);

            if (res == null)
            {
                throw new Exception("Error inserting user.");
            }

            UserRolService userRolService = new UserRolService(DbConnection, Transaction);
            userRolService.Insert(new UserRolDTO
            {
                UserId = res.UserId,
                RolId = RolesType.Guest,
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
                $@"UPDATE [dbo].[TR_Users]
                        SET UserName = @UserName,
                            Email = @Email,
                            Available = @Available,
                            LastUpdated = @dateTime
                        WHERE UserId = @UserId
                        SELECT * FROM [dbo].[TR_Users] WHERE UserId = @UserId
                        ",
                new
                {
                    data.UserId,
                    data.UserName,
                    Email = data.Email.ToLower(),
                    data.Available,
                    dateTime = DateTime.UtcNow
                }, Transaction);
        }

        public bool ValidPassword(UserLogin userLogin)
        {
            userLogin.Password = DecryptToBase64(userLogin.Password);
            var userInDB = GetByEmail(userLogin.Email);
            if (userInDB == null)
            {
                return false;
            }
            var user = GetUpdateById(userInDB.UserId);

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
                $@"SELECT UserId, UserName FROM [dbo].[TR_Users]
                    WHERE UserName LIKE @userName AND Available = 1
                    ORDER BY UserName
                    OFFSET {(page - 1) * 10} ROWS FETCH NEXT 10 ROWS ONLY",
                new { userName = $"%{userName}%" }, Transaction);
        }

        public UserDTO? GetByEmail(string email)
        {
            return DbConnection.QueryFirstOrDefault<UserDTO>(
                @"SELECT * FROM [dbo].[TR_Users] WHERE Email = @email", new { email = email.ToLower() }, Transaction);
        }
    }
}