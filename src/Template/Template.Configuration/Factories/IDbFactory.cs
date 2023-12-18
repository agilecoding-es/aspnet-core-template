using System.Data.Common;

namespace Template.Configuration.Factories
{
    public interface IDbFactory
    {
        DbConnection CreateConnection(string connectionString);
    }
}
