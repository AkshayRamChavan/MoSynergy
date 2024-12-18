using Microsoft.Data.SqlClient;
using System.Data;

namespace MoSynergy.Data
{
    public class DapperDbContext
    {
        private readonly IConfiguration _configuration;
        private readonly string ConnectionStrings;
        public DapperDbContext(IConfiguration configuration)
        {
            this._configuration = configuration;
            this.ConnectionStrings = this._configuration.GetConnectionString("Connection");

            
        }
        public IDbConnection CreateConnection() => new SqlConnection(ConnectionStrings);

    }
}
