using System.Data;
using System.Data.SqlClient;

namespace ResultViewer.Server.Context
{
    public class DbContext
    {
        private readonly IConfiguration _configuration;
        private readonly string? _connectionString;

        public DbContext(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
        }

        public IDbConnection CreateConnection() => new SqlConnection(_connectionString);
    }
}
