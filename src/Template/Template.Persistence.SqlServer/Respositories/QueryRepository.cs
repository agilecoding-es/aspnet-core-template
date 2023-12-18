using Microsoft.Extensions.Configuration;
using Template.Common;

namespace Template.Persistence.SqlServer.Respositories
{
    public abstract class QueryRepository
    {

        protected readonly string _connectionString;

        public QueryRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString(Constants.Configuration.ConnectionString.DefaultConnection) ?? throw new ArgumentNullException(nameof(configuration));
        }
    }
}
