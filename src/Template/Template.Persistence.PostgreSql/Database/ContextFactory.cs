using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Template.Common;

namespace Template.Persistence.PosgreSql.Database
{
    public class ContextFactory : IDesignTimeDbContextFactory<Context>
    {
        public Context CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = configuration.GetConnectionString(Constants.Configuration.ConnectionString.DefaultConnection);

            var optionsBuilder = new DbContextOptionsBuilder<Context>();
            optionsBuilder.UseNpgsql(connectionString);

            return new Context(optionsBuilder.Options);
        }
    }
}
