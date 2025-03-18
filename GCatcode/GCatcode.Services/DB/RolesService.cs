using Dapper;
using GCatcode.SQLServerDatabase.Dtos;
using GCatcode.SQLServerDatabase.Models;
using Microsoft.Data.SqlClient;

namespace GCatcode.Repository.DB
{
    public class RolesService : BaseTableService<RolDTO>
    {
        public RolesService(string connectionString) : base("Roles", "RolId", connectionString)
        {
        }

        public RolDTO Update(RolDTO item)
        {
            using(SqlConnection connection = new SqlConnection(_connectionString))
            {
                return connection.QueryFirst<RolDTO>(
                    $@"UPDATE [dbo].[{_tableName}]
                        SET Name = @Name,
                            Description = @Description,
                            Available = 1,
                            LastUpdated = @dateTime
                        WHERE RolId = @RolId
                        SELECT * FROM [dbo].[{_tableName}] WHERE RolId = @RolId
                        ",
                    new
                    {
                        item.RolId,
                        item.Name,
                        item.Description,
                        dateTime = DateTime.Now
                    });
            }
        }
    }
}