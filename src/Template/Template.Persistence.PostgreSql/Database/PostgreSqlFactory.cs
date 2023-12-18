using Npgsql;
using System.Data.Common;
using Template.Configuration.Factories;

namespace Template.Persistence.PostgreSql.Database
{
    public class PostgreSqlFactory : IDbFactory
    {
        public DbConnection CreateConnection(string connectionString)
        {
            _ = connectionString ?? throw new ArgumentNullException(nameof(connectionString));

            return new NpgsqlConnection(connectionString);
        }
    }
}
