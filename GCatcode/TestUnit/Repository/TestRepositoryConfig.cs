using Microsoft.Data.SqlClient;

namespace TestUnit.Repository
{
    public class TestRepositoryConfig
    {
        protected readonly SqlConnection _connection;

        [AssemblyInitialize]
        public static void AssemblyInit(TestContext context)
        {
            // Este método se llama una vez para el ensamblado de pruebas, antes de que se ejecuten las pruebas.

        }
        public TestRepositoryConfig()
        {
            _connection = new SqlConnection("Server=localhost,1433;Initial Catalog=BaseLine;Persist Security Info=False;User ID=sa;Password=GcatcodeDB2521.;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;Connection Timeout=30;");
            if (_connection.State != System.Data.ConnectionState.Open)
            {
                _connection.Open();
            }
        }
    }
}