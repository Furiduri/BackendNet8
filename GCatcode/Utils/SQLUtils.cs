using System.Reflection;

namespace GCatcode.Utils
{
    public class SQLUtils
    {
        public static string ParseWere(object? filters)
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
    }
}