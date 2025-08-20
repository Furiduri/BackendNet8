using Microsoft.Data.SqlClient;

namespace GCatcode.Repository.DB
{
    public abstract class DBService
    {
        protected readonly SqlConnection DbConnection;

        public DBService(SqlConnection dbConnection)
        {
            DbConnection = dbConnection ?? throw new ArgumentNullException(nameof(dbConnection));
            CheckConnection();
        }

        private void CheckConnection()
        {
            if (DbConnection.State != System.Data.ConnectionState.Open)
            {
                DbConnection.Open();
            }
        }

        public DBService(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentException("Connection string cannot be null or empty.", nameof(connectionString));
            }
            DbConnection = new SqlConnection(connectionString);
            CheckConnection();
        }
    }
}