using Dapper;
using GCatcode.Utils;
using Microsoft.Data.SqlClient;

namespace GCatcode.Repository.DB.RolServices
{
    public class RolesService : DBService, IDBService<RolDTO, RolInsert, RolUpdate>
    {
        public RolesService(string connectionString)
            : base(connectionString)
        {
        }

        public RolesService(SqlConnection sqlConnection)
            : base(sqlConnection)
        {
        }

        public RolDTO? GetById(int id)
        {
            return DbConnection.QueryFirst<RolDTO>($@"SELECT * FROM [dbo].[Roles] WHERE RolId = @id", new { id });
        }

        public IEnumerable<RolDTO> Get(int maxItems = 100, int page = 1, object? filters = null)
        {
            return DbConnection.Query<RolDTO>($@"SELECT TOP {maxItems} * FROM [dbo].[Roles] WHERE 1 = 1 {SQLUtils.ParseWere(filters)}", filters);
        }

        public RolDTO Delete(int id)
        {
            return DbConnection.QueryFirst<RolDTO>(
                   $@"UPDATE [dbo].[Roles]
                        SET Available = 0,
                            LastUpdated = @dateTime
                        WHERE RolId = @RolId
                        SELECT * FROM [dbo].[Roles] WHERE RolId = @id
            ",
            new
            {
                id,
                dateTime = DateTime.UtcNow
            });
        }

        public RolDTO Update(RolUpdate data)
        {
            return DbConnection.QueryFirst<RolDTO>(
                $@"UPDATE [dbo].[Roles]
                        SET Name = @Name,
                            Description = @Description,
                            Available = @Available,
                            LastUpdated = @dateTime
                        WHERE RolId = @RolId
                        SELECT * FROM [dbo].[Roles] WHERE RolId = @RolId
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

        public RolDTO Insert(RolInsert data)
        {
            return DbConnection.QueryFirst<RolDTO>(
                $@"
                        INSERT INTO [dbo].[Roles] (Name, Description)
                        VALUES (@Name, @Description)
                        SELECT * FROM [dbo].[Roles] WHERE RolId = @@IDENTITY
                        ",
                new
                {
                    data.Name,
                    data.Description
                });
        }
    }
}