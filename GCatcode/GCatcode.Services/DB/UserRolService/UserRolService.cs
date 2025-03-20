using Dapper;
using GCatcode.Repository.DB.RolService;
using Microsoft.Data.SqlClient;

namespace GCatcode.Repository.DB.UserRolService
{
    public class UserRolService
    {

        public UserRolService(string connectionString)
        {
            ConnectionString = connectionString;
        }

        private readonly string ConnectionString;

        public UserRolDTO Delete(int UserId, int RolId)
        {
            using var DbConnection = new SqlConnection(ConnectionString);
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

        public IEnumerable<RolUpdate> GetRolsByUserId(int userId)
        {
            using var DbConnection = new SqlConnection(ConnectionString);
            return DbConnection.Query<RolUpdate>(
                @"SELECT r.* FROM [dbo].[UserRoles] ur
                    INNER JOIN [dbo].[Roles] r ON ur.RolId = r.RolId AND r.Available = 1
                    WHERE ur.UserId = @userId
                ",
                new { userId });
        }

        public UserRolDTO Insert(UserRolDTO data)
        {
            using var DbConnection = new SqlConnection(ConnectionString);
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