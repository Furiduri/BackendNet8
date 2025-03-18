using Dapper;
using GCatcode.Utils;
using Microsoft.Data.SqlClient;

namespace GCatcode.Repository.DB.RolService
{
    public class RolesService : IDBService<RolDTO, RolInsert, RolUpdate>
    {
        public RolesService(string connectionString)
        {
            DbConnectionString = connectionString;
            TableName = "Roles";
        }

        private readonly string DbConnectionString;
        private readonly string TableName;

        public RolDTO? GetById(int id)
        {
            using (SqlConnection connection = new SqlConnection(DbConnectionString))
            {
                return connection.QueryFirst<RolDTO>($@"SELECT * FROM [dbo].[{TableName}] WHERE RolId = @id", new { id });
            }
        }

        public IEnumerable<RolDTO> Get(int maxItems = 100, int page = 1, object? filters = null)
        {
            using (SqlConnection connection = new SqlConnection(DbConnectionString))
            {
                return connection.Query<RolDTO>($@"SELECT TOP {maxItems} * FROM [dbo].[{TableName}] WHERE 1 = 1 {SQLUtils.ParseWere(filters)}");
            }
        }

        public RolDTO Delete(int id)
        {
            throw new NotImplementedException();
        }

        public RolDTO Update(RolUpdate data)
        {
            using (SqlConnection connection = new SqlConnection(DbConnectionString))
            {
                return connection.QueryFirst<RolDTO>(
                    $@"UPDATE [dbo].[{TableName}]
                        SET Name = @Name,
                            Description = @Description,
                            Available = @Available,
                            LastUpdated = @dateTime
                        WHERE RolId = @RolId
                        SELECT * FROM [dbo].[{TableName}] WHERE RolId = @RolId
                        ",
                    new
                    {
                        data.RolId,
                        data.Name,
                        data.Description,
                        data.Available,
                        dateTime = DateTime.UtcNow
                    });
            }
        }

        public RolDTO Insert(RolInsert data)
        {
            throw new NotImplementedException();
        }
    }
}