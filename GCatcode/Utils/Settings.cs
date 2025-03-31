using Microsoft.Extensions.Configuration;

namespace GCatcode.Utils
{
    public class Settings
    {
        public static string GetBaseDBConnection(IConfiguration configuration)
        {
           return configuration.GetConnectionString("DataBase");
        }
    }
}