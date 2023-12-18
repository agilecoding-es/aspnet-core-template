using Microsoft.Data.SqlClient;
using System.Data.Common;
using Template.Configuration.Factories;

namespace Template.Persistence.SqlServer.Database
{
    public class SqlServerFactory : IDbFactory
    {
        public DbConnection CreateConnection(string connectionString)
        {
            _ = connectionString ?? throw new ArgumentNullException(nameof(connectionString));

            return new SqlConnection(connectionString);
        }
    }
}
