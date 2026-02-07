using Microsoft.Data.SqlClient;
using System.Data;

namespace GCatcode.Repository.DB
{
    public abstract class DBService
    {
        protected readonly SqlConnection DbConnection;
        protected IDbTransaction Transaction;

        public DBService(SqlConnection dbConnection, IDbTransaction transaction)
        {
            DbConnection = dbConnection ?? throw new ArgumentNullException(nameof(dbConnection));
            CheckConnection();
            Transaction = transaction;
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
            Transaction = null;
        }
    }
}