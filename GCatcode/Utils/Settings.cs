using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace GCatcode.Utils
{
    public class Settings
    {
        public static SqlConnection GetSqlDBConnection(IConfiguration configuration)
        {
            var connetion = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
            if (connetion == null || connetion.ConnectionString == null)
            {
                throw new ArgumentNullException("Connection string is not configured properly.");
            }
            if (connetion.State != System.Data.ConnectionState.Open)
            {
                connetion.Open();
            }
            return connetion;
        }
    }
}