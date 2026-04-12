using Dapper;
using GCatcode.Services.DB.RolServices.Models;
using GCatcode.Utils;
using Microsoft.Data.SqlClient;
using System.Data;

namespace GCatcode.Services.DB.RolServices
{
    public class RolesService : DBService
    {
        public RolesService(SqlConnection sqlConnection)
            : base(sqlConnection, null)
        {
        }

        public RolesService(SqlConnection sqlConnection, IDbTransaction transaction)
            : base(sqlConnection, transaction)
        {
        }

        public RolDTO? GetById(int id)
        {
            return DbConnection.QueryFirstOrDefault<RolDTO>($@"SELECT * FROM [dbo].[CL_Roles] WHERE RolId = @id", new { id }, Transaction);
        }

        public IEnumerable<RolDTO> Get(int page = 1, RolFilter? filters = null, int maxItems = 100)
        {
            return DbConnection.Query<RolDTO>($@"SELECT *
                    FROM [dbo].[CL_Roles] WHERE 1 = 1 {SQLUtils.ParseWere(filters)}
                    ORDER BY RolId
                    OFFSET {((page - 1) * maxItems)} ROWS FETCH NEXT {maxItems} ROWS ONLY"
                    , filters, Transaction);
        }

        public RolDTO? Delete(int rolId)
        {
            return DbConnection.QueryFirstOrDefault<RolDTO>(
                   $@"UPDATE [dbo].[CL_Roles]
                        SET Available = 0,
                            LastUpdated = @dateTime
                        WHERE RolId = @RolId
                        SELECT * FROM [dbo].[CL_Roles] WHERE RolId = @RolId
            ",
            new
            {
                rolId,
                dateTime = DateTime.UtcNow
            }, Transaction);
        }

        public RolDTO? Update(RolUpdate data)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));

            return DbConnection.QueryFirstOrDefault<RolDTO>(
                $@"UPDATE [dbo].[CL_Roles]
                        SET Name = @Name,
                            Description = @Description,
                            Available = @Available,
                            LastUpdated = @dateTime
                        WHERE RolId = @RolId
                        SELECT * FROM [dbo].[CL_Roles] WHERE RolId = @RolId
                        ",
                new
                {
                    data.RolId,
                    data.Name,
                    data.Description,
                    data.Available,
                    dateTime = DateTime.UtcNow
                }, Transaction);
        }

        public RolDTO? Add(RolInsert data)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));

            return DbConnection.QueryFirstOrDefault<RolDTO>(
                $@"
                        INSERT INTO [dbo].[CL_Roles] (Name, Description)
                        VALUES (@Name, @Description)
                        SELECT * FROM [dbo].[CL_Roles] WHERE RolId = @@IDENTITY
                        ",
                new
                {
                    data.Name,
                    data.Description
                }, Transaction);
        }

        public IEnumerable<RolItem> GetRolesByUserId(int userId)
        {
            return DbConnection.Query<RolItem>($@"
                    SELECT r.*
                    FROM [dbo].[CL_Roles] r
                    INNER JOIN [dbo].[RL_UserRoles] ur ON r.RolId = ur.RolId
                    WHERE ur.UserId = @userId
                    ", new { userId }, Transaction);
        }
    }
}