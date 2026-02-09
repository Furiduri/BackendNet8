using Dapper;
using GCatcode.Repository.DB.RolServices.Models;
using GCatcode.Repository.DB.UserRolServices.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace GCatcode.Repository.DB.UserRolServices
{
    public class UserRolService : DBService
    {
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
                @"UPDATE [dbo].[RL_UserRoles]
                    SET Available = 0,
                        LastUpdated = @dateTime
                    WHERE UserId = @UserId AND RolId = @RolId
                    SELECT * FROM [dbo].[RL_UserRoles] WHERE UserId = @UserId AND RolId = @RolId
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
                @"SELECT r.* FROM [dbo].[RL_UserRoles] ur
                    INNER JOIN [dbo].[CL_Roles] r ON ur.RolId = r.RolId AND r.Available = 1
                    WHERE ur.UserId = @userId
                ",
                new { userId }, Transaction);
        }

        public UserRolDTO Insert(UserRolDTO data)
        {
            return DbConnection.QueryFirst<UserRolDTO>(
                @"IF EXISTS(SELECT * FROM [dbo].[RL_UserRoles] WHERE UserId = @UserId AND RolId = @RolId) BEGIN
	                UPDATE [dbo].[RL_UserRoles]
                    SET Available = 1,
                        LastUpdated = @dateTime
                    WHERE UserId = @UserId AND RolId = @RolId
                END ELSE BEGIN
	                INSERT INTO [dbo].[RL_UserRoles] (UserId, RolId, Available, LastUpdated) VALUES (@UserId, @RolId, 1, @dateTime)
                END
                SELECT * FROM [dbo].[RL_UserRoles] WHERE UserId = @UserId AND RolId = @RolId", new { data.UserId, data.RolId, dateTime = DateTime.UtcNow }, Transaction);
        }
    }
}