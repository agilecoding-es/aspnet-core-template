using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Data.SqlClient;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Respawn;
using Respawn.Graph;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Template.Configuration;
using Template.MvcWebApp.IntegrationTests.Attributes;
using Template.Persistence.Database;
using static Template.Configuration.Constants.Configuration;

namespace Template.MvcWebApp.IntegrationTests
{
    internal class WebAppFactory : WebApplicationFactory<Program>
    {
        private const string NON_DEVELOPMENT_DB_EXCEPTION = "Cannot RESET Database of non development Database. Check your appsettings.";

        #region Static

        private static WebAppFactory factory = default!;

        public static WebAppFactory FactoryInstance
        {
            get
            {
                if (factory == null)
                    factory = Create().GetAwaiter().GetResult();

                return factory;
            }
        }

        private static class FactoryConfiguration
        {
            public static string ConnectionString => FactoryInstance.Configuration.GetConnectionString(Constants.Configuration.ConnectionString.DEFAULT_CONNECTION);

            public static AppSettings Settings => FactoryInstance.Services.GetService<IOptions<AppSettings>>().Value;
        }

        public static async Task<WebAppFactory> Create()
        {
            var factory = new WebAppFactory();

            await factory.Connection.OpenAsync();
            await factory.InitializeRespawner();

            return factory;
        }

        public static async Task ResetDatabaseAsync()
        {
            if (IsNotLocalDb())
                throw new Exception(NON_DEVELOPMENT_DB_EXCEPTION);

            await FactoryInstance.Respawner.ResetAsync(FactoryConfiguration.ConnectionString);
        }

        private static bool IsNotLocalDb() =>
           !FactoryConfiguration.ConnectionString.Contains("localhost") &&
           !FactoryConfiguration.ConnectionString.Contains("SQLEXPRESS") &&
           !FactoryConfiguration.ConnectionString.Contains("MSSQLSERVER") &&
           !FactoryConfiguration.ConnectionString.Contains("ats.sql") &&
           !FactoryConfiguration.ConnectionString.Contains("(local)") &&
           !FactoryConfiguration.ConnectionString.Contains("(localdb)") &&
           !FactoryConfiguration.ConnectionString.Contains("127.0.0.1") &&
           !FactoryConfiguration.ConnectionString.Contains("ats.mysql");

        #endregion

        private DbConnection Connection = default!;
        private Respawner Respawner = default!;
        private IConfiguration Configuration = default!;

        public WebAppFactory() : base()
        {
            Configuration = new ConfigurationBuilder()
                                .SetBasePath(Directory.GetCurrentDirectory())
                                .AddJsonFile("appsettings.json")
                                .AddEnvironmentVariables()
                                .Build();
            Connection = new SqlConnection(Configuration.GetConnectionString(ConnectionString.DEFAULT_CONNECTION));
            SharedHttpClient = CreateClient();
        }

        public HttpClient SharedHttpClient { get; private set; } = default!;


        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            string parentDirectories = string.Format("..{0}..{0}..{0}..{0}", Path.DirectorySeparatorChar);
            var contentRootPath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), parentDirectories, PresentationAssembly.Assembly.GetName().Name));
            

            builder.ConfigureServices(services =>
            {
                ConfigureMocks(services);
            })
            .UseConfiguration(Configuration)
            .UseContentRoot(contentRootPath)
            .UseEnvironment("Development");
        }

        private void ConfigureMocks(IServiceCollection services) { }

        private async Task InitializeRespawner() =>
            Respawner = await Respawner.CreateAsync(
                        Connection,
                        new RespawnerOptions()
                        {
                            DbAdapter = DbAdapter.SqlServer,
                            SchemasToExclude = new string[]
                            {
                                Context.DbSchema.log.ToString()
                            },
                            TablesToIgnore = new Table[]
                            {
                                "__EFMigrationsHistory"
                            }
                        });

    }
}
