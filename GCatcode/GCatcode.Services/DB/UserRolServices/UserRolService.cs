using Dapper;
using GCatcode.Repository.DB.RolServices;
using Microsoft.Data.SqlClient;

namespace GCatcode.Repository.DB.UserRolServices
{
    public class UserRolService : DBService
    {
        public UserRolService(string connectionString)
            : base(connectionString)
        {
        }

        public UserRolService(SqlConnection dbConnection) : base(dbConnection)
        {
        }

        public UserRolDTO Delete(int UserId, int RolId)
        {
            return DbConnection.QueryFirst<UserRolDTO>(
                @"UPDATE [dbo].[UserRoles]
                    SET Available = 0,
                        LastUpdated = @dateTime
                    WHERE UserId = @UserId AND RolId = @RolId
                    SELECT * FROM [dbo].[UserRoles] WHERE UserId = @UserId AND RolId = @RolId
                ",
                new
                {
                    UserId,
                    RolId,
                    dateTime = DateTime.UtcNow
                });
        }

        public IEnumerable<RolItem> GetRolsByUserId(int userId)
        {
            return DbConnection.Query<RolItem>(
                @"SELECT r.* FROM [dbo].[UserRoles] ur
                    INNER JOIN [dbo].[Roles] r ON ur.RolId = r.RolId AND r.Available = 1
                    WHERE ur.UserId = @userId
                ",
                new { userId });
        }

        public UserRolDTO Insert(UserRolDTO data)
        {
            return DbConnection.QueryFirst<UserRolDTO>(
                @"IF EXISTS(SELECT * FROM UserRoles WHERE UserId = @UserId AND RolId = @RolId) BEGIN
	                UPDATE [dbo].[UserRoles]
                    SET Available = 1
                        LastUpdated = @dateTime
                    WHERE UserId = @UserId AND RolId = @RolId
                END ELSE BEGIN
	                INSERT INTO [dbo].[UserRoles] (UserId, RolId, Available) VALUES (@UserId, @RolId, 1)
                END
                SELECT * FROM [dbo].[UserRoles] WHERE UserId = @UserId AND RolId = @RolId", new { data.UserId, data.RolId, dateTime = DateTime.UtcNow });
        }
    }
}