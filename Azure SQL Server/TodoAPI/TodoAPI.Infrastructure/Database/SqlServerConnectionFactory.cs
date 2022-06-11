using Microsoft.Data.SqlClient;
using System.Data;

namespace TodoAPI.Infrastructure.Database
{
    public class SqlServerConnectionFactory
    {
        private readonly string _connectionString;

        public SqlServerConnectionFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IDbConnection Connect()
        {
            return new SqlConnection(_connectionString);
        }
    }
}
