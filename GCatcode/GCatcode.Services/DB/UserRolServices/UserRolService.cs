using Dapper;
using GCatcode.Repository.DB.RolServices;
using Microsoft.Data.SqlClient;
using System.Data;

namespace GCatcode.Repository.DB.UserRolServices
{
    public class UserRolService : DBService
    {
        public UserRolService(string connectionString)
            : base(connectionString)
        {
        }

        public UserRolService(SqlConnection dbConnection) 
            : base(dbConnection, null)
        {
        }

        public UserRolService(SqlConnection dbConnection, IDbTransaction transaction) 
            : base(dbConnection, transaction)
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
                }, Transaction);
        }

        public IEnumerable<RolItem> GetRolsByUserId(int userId)
        {
            return DbConnection.Query<RolItem>(
                @"SELECT r.* FROM [dbo].[UserRoles] ur
                    INNER JOIN [dbo].[Roles] r ON ur.RolId = r.RolId AND r.Available = 1
                    WHERE ur.UserId = @userId
                ",
                new { userId }, Transaction);
        }

        public UserRolDTO Insert(UserRolDTO data)
        {
            return DbConnection.QueryFirst<UserRolDTO>(
                @"IF EXISTS(SELECT * FROM UserRoles WHERE UserId = @UserId AND RolId = @RolId) BEGIN
	                UPDATE [dbo].[UserRoles]
                    SET Available = 1,
                        LastUpdated = @dateTime
                    WHERE UserId = @UserId AND RolId = @RolId
                END ELSE BEGIN
	                INSERT INTO [dbo].[UserRoles] (UserId, RolId, Available, LastUpdated) VALUES (@UserId, @RolId, 1, @dateTime)
                END
                SELECT * FROM [dbo].[UserRoles] WHERE UserId = @UserId AND RolId = @RolId", new { data.UserId, data.RolId, dateTime = DateTime.UtcNow }, Transaction);
        }
    }
}