using Microsoft.Data.SqlClient;
using System.Data;
using System.Reflection;

namespace GCatcode.Utils
{
    public class SQLUtils
    {
        public static string ParseWere(object filters)
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

        public static SqlParameter[] GetSqlParameters(object parameters)
        {
            try
            {
                List<SqlParameter> listParameters = new List<SqlParameter>();
                if (parameters != null)
                {
                    PropertyInfo[] propiedades = parameters.GetType().GetProperties();
                    foreach (var prop in propiedades)
                    {
                        object value = prop.GetValue(parameters) ?? DBNull.Value;
                        listParameters.Add(new SqlParameter($"@{prop.Name}", value)
                        {
                            IsNullable = (value == DBNull.Value),
                            DbType = GetDbType(prop.PropertyType)
                        });
                    }
                }
                return listParameters.ToArray();
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private static DbType GetDbType(Type type)
        {
            if (type == typeof(string)) return DbType.String;
            if (type == typeof(short)) return DbType.Int16;
            if (type == typeof(short?)) return DbType.Int16;
            if (type == typeof(int)) return DbType.Int32;
            if (type == typeof(int?)) return DbType.Int32;
            if (type == typeof(bool)) return DbType.Boolean;
            if (type == typeof(bool?)) return DbType.Boolean;
            if (type == typeof(DateTime)) return DbType.DateTime;
            if (type == typeof(DateTime?)) return DbType.DateTime;
            if (type == typeof(decimal)) return DbType.Decimal;
            if (type == typeof(decimal?)) return DbType.Decimal;
            if (type == typeof(double)) return DbType.Double;
            if (type == typeof(double?)) return DbType.Double;
            if (type == typeof(float)) return DbType.Single;
            if (type == typeof(float?)) return DbType.Single;
            if (type == typeof(long)) return DbType.Int64;
            if (type == typeof(long?)) return DbType.Int64;
            // Agrega más tipos según sea necesario
            return DbType.Object;
        }

    }
}