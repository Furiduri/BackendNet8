using Dapper;
using Microsoft.Data.SqlClient;
using System;
using System.Drawing;
using System.Linq;
using System.Reflection;

namespace GCatcode.Repository.DB
{
    public class BaseTableService<T> : IDBService<T>
    {
        protected string _tableName;
        protected string _keyName;
        protected string _connectionString;

        public BaseTableService(string tableName, string keyName, string connectionString)
        {
            _tableName = tableName;
            _connectionString = connectionString;
            _keyName = keyName;
        }

        public virtual void Delete(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Query($@"UPDATE [dbo].[{_tableName}]
                                    SET Available = 0,
                                        LastUpdated = @dateTime
                                WHERE {_keyName} = @id", new
                {
                    id,
                    dateTime = DateTime.UtcNow,
                });
            }
        }

        public virtual List<T> Get(int maxItems = 100, object? filters = null)
        {
            var list = new List<T>();
            string whereFilters = ParseWere(filters);
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                list.AddRange(connection.Query<T>($@"SELECT TOP {maxItems} *
                                            FROM [dbo].[{_tableName}]
                                            WHERE 1 = 1 {whereFilters}",
                    param: filters));
            }
            return list;
        }

        protected string ParseWere(object? filters)
        {
            string were = "";
            if (filters != null)
            {
                PropertyInfo[] propiedades = filters.GetType().GetProperties();
                foreach (var prop in propiedades)
                {
                    were += $" AND {prop.Name} = @{prop.Name} ";
                }
            }
            return were;
        }

        public virtual T? GetById(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                return connection.QueryFirstOrDefault<T>($@"SELECT *
                                                            FROM [dbo].[{_tableName}]
                                                            WHERE {_keyName} = @id"
                            , new
                            {
                                id
                            });
            }
        }
    }
}