using Acheve.AspNetCore.TestHost.Security;
using Acheve.TestHost;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Respawn;
using Respawn.Graph;
using System.Data.Common;
using Template.Application.Features.LoggingContext.Contracts;
using Template.Common;
using Template.Common.TypesExtensions;
using Template.Configuration;
using Template.Configuration.Factories;
using Template.Configuration.Setup;
using Template.Persistence.PostgreSql.Database;
using Template.Persistence.SqlServer.Database;

namespace Template.WebApp.IntegrationTests
{
    internal class WebAppFactory : WebApplicationFactory<Program>
    {
        private const string NON_DEVELOPMENT_DB_EXCEPTION = "Cannot RESET Database of non development Database. Check your appsettings.";

        #region Static

        private static Dictionary<string, WebAppFactory> factories = new Dictionary<string, WebAppFactory>();

        public static WebAppFactory GetFactoryInstance(string connectionStringName = Constants.Configuration.ConnectionString.DefaultConnection)
        {
            if (factories.IsNullOrEmpty() || !factories.Any(f => f.Key == connectionStringName))
                factories.Add(connectionStringName, Create(connectionStringName).GetAwaiter().GetResult());

            return factories.First(f => f.Key == connectionStringName).Value;
        }

        internal static class FactoryConfiguration
        {
            public static string ConnectionString
            {
                get
                {
                    var instance = GetFactoryInstance();
                    return instance.Configuration.GetConnectionString(instance.ConnectionStringName);
                }
            }
            public static DbConnection Connection => GetFactoryInstance().Connection;

            public static AppSettings Settings => GetFactoryInstance().Services.GetService<IOptions<AppSettings>>().Value;
        }

        public static async Task<WebAppFactory> Create(string connectionStringName = null)
        {
            connectionStringName = !string.IsNullOrWhiteSpace(connectionStringName) ? connectionStringName : Constants.Configuration.ConnectionString.DefaultConnection;

            var factory = new WebAppFactory();

            factory.ConnectionStringName = connectionStringName;
            factory.Connection = factory.DbFactory.CreateConnection(factory.Configuration.GetConnectionString(connectionStringName));

            var logger = factory.Services.GetRequiredService<ILogger<Program>>();
            logger.LogInformation("Factory created");

            await factory.Connection.OpenAsync();
            await factory.InitializeRespawner();

            return factory;
        }

        public void CreateDbFactory()
        {
            var db = Configuration.Get<DbOptions>() ?? throw new ArgumentNullException(nameof(DbOptions));

            if (db.Provider == Constants.Configuration.DatabaseProvider.SqlServerProvider)
                DbFactory = new SqlServerFactory();
            else
                DbFactory = new PostgreSqlFactory();
        }


        public static void ResetDatabase(WebAppFactory instance = null)
        {
            if (IsNotLocalDb())
                throw new Exception(NON_DEVELOPMENT_DB_EXCEPTION);

            if (instance != null)
            {
                instance.Respawner.ResetAsync(FactoryConfiguration.Connection).GetAwaiter().GetResult();
            }
            else
            {
                GetFactoryInstance().Respawner.ResetAsync(FactoryConfiguration.Connection).GetAwaiter().GetResult();
            }
        }

        public static int GetExceptionsCount() => GetFactoryInstance()
                                                    .Services
                                                    .GetRequiredService<IExceptionQueryRepository>()
                                                    .GetExceptionsCountAsync(CancellationToken.None).GetAwaiter().GetResult();

        private static bool IsNotLocalDb() =>
           !FactoryConfiguration.ConnectionString.Contains("localhost") &&
           !FactoryConfiguration.ConnectionString.Contains("SQLEXPRESS") &&
           !FactoryConfiguration.ConnectionString.Contains("MSSQLSERVER") &&
           !FactoryConfiguration.ConnectionString.Contains("(local)") &&
           !FactoryConfiguration.ConnectionString.Contains("(localdb)") &&
           !FactoryConfiguration.ConnectionString.Contains("127.0.0.1") &&
           !FactoryConfiguration.ConnectionString.Contains("TemplateAppIntegrationTests");

        #endregion

        private IConfiguration Configuration = default!;
        private string ConnectionStringName = default!;
        private IDbFactory DbFactory = default!;
        private DbConnection Connection = default!;
        private Respawner Respawner = default!;

        public WebAppFactory() : base()
        {
            Configuration = new ConfigurationBuilder()
                                .SetBasePath(Directory.GetCurrentDirectory())
                                .AddJsonFile("appsettings.integrationtests.json")
                                .AddUserSecrets<WebAppFactory>()
                                .AddEnvironmentVariables()
                                .Build();

            CreateDbFactory();
        }

        public RequestBuilder CreateRequest(string path) => Server.CreateRequest(path);

        public T GetService<T>()
        {
            using var scope = Server.Services.CreateScope();
            return scope.ServiceProvider.GetService<T>();
        }

        public async Task ExecuteInScopeAsync(Func<IServiceProvider, Task> action)
        {
            //using var scope = Server.Services.GetService<IServiceScopeFactory>().CreateScope();
            using var scope = Server.Services.CreateScope();
            await action(scope.ServiceProvider);
        }

        public async Task ExecuteInScopeAsync<T>(Func<T, Task> action)
        {
            //using var scope = Server.Services.GetService<IServiceScopeFactory>().CreateScope();
            using var scope = Server.Services.CreateScope();
            await action(scope.ServiceProvider.GetService<T>());
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            string parentDirectories = string.Format("..{0}..{0}..{0}..{0}", Path.DirectorySeparatorChar);
            var contentRootPath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), parentDirectories, PresentationAssembly.Assembly.GetName().Name));

            builder.ConfigureServices(services =>
            {
                services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = TestServerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = TestServerDefaults.AuthenticationScheme;
                })
                .AddCookie(Constants.Configuration.Cookies.AuthCookieName)
                .AddTestServer();

                services.AddPostgreSql(ConnectionStringName);

                ConfigureTestDependencies(services);
                ConfigureMocks(services);
            })
            .UseConfiguration(Configuration)
            .UseContentRoot(contentRootPath)
            .UseEnvironment("Development")
            .UseTestServer()
            .UseSetting("https_port", "443");
        }

        protected override void ConfigureClient(HttpClient client)
        {
            // Configura la BaseAddress para todas las solicitudes
            client.BaseAddress = new Uri("https://localhost");
            ClientOptions.BaseAddress = new Uri("https://localhost");
        }


        private async Task InitializeRespawner() =>
            Respawner = await Respawner.CreateAsync(
                        Connection,
                        new RespawnerOptions()
                        {
                            //DbAdapter = DbAdapter.SqlServer,
                            DbAdapter = DbAdapter.Postgres,
                            SchemasToExclude = new string[]
                            {
                                DbSchema.log.ToString()
                            },
                            TablesToIgnore = new Table[]
                            {
                                "__EFMigrationsHistory"
                            }
                        });

        private void ConfigureTestDependencies(IServiceCollection services)
        {
        }

        private void ConfigureMocks(IServiceCollection services) { }

    }
}
