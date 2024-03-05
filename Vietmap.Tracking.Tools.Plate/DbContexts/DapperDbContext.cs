using Microsoft.Data.SqlClient;
using System.Data;

namespace Vietmap.Tracking.Tools.Plate.DbContexts
{
    public class DapperDbContext
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        private readonly ILogger<DapperDbContext> _logger;

        public DapperDbContext(IConfiguration configuration, ILogger<DapperDbContext> logger)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("SqlConnection");
            _logger = logger;
        }

        public async Task<IDbConnection> CreateConnection()
        {
            try
            {
                var conn = new SqlConnection(_connectionString);
                await conn.OpenAsync();
                return conn;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "DapperDbContext - CreateConnection - Error - {Message}", ex.Message);
                throw;
            }
        }
    }
}
