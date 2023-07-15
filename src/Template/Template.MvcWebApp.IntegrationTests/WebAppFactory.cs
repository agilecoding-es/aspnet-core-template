﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Respawn;
using Respawn.Graph;
using System.Data.Common;
using Template.Common;
using Template.Common.Extensions;
using Template.Configuration;
using Template.MvcWebApp.IntegrationTests.Fixtures;
using Template.MvcWebApp.IntegrationTests.Queries;
using Template.Persistence.Database;

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

        internal static class FactoryConfiguration
        {
            public static string ConnectionString => FactoryInstance.Configuration.GetConnectionString(Constants.Configuration.ConnectionString.DefaultConnection.Value);

            public static AppSettings Settings => FactoryInstance.Services.GetService<IOptions<AppSettings>>().Value;
        }

        public static async Task<WebAppFactory> Create()
        {
            var factory = new WebAppFactory();

            await factory.Connection.OpenAsync();
            await factory.InitializeRespawner();

            return factory;
        }

        public static void ResetDatabase()
        {
            if (IsNotLocalDb())
                throw new Exception(NON_DEVELOPMENT_DB_EXCEPTION);

            FactoryInstance.Respawner.ResetAsync(FactoryConfiguration.ConnectionString).GetAwaiter().GetResult();
        }

        public static int GetExceptionsCount() => ExceptionsQueries
                                                    .CreateConnection(FactoryConfiguration.ConnectionString)
                                                    .GetExceptionsCountAsync().GetAwaiter().GetResult();

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

        private IConfiguration Configuration = default!;
        private DbConnection Connection = default!;
        private Respawner Respawner = default!;

        public WebAppFactory() : base()
        {
            Configuration = new ConfigurationBuilder()
                                .SetBasePath(Directory.GetCurrentDirectory())
                                .AddJsonFile("appsettings.json")
                                .AddEnvironmentVariables()
                                .Build();
            Connection = new SqlConnection(Configuration.GetConnectionString(Constants.Configuration.ConnectionString.DefaultConnection.Value));
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
                ConfigureTestDependencies(services);
                ConfigureMocks(services);
            })
            .UseConfiguration(Configuration)
            .UseContentRoot(contentRootPath)
            .UseEnvironment("Development");
        }

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

        private void ConfigureTestDependencies(IServiceCollection services)
        {
            services.AddScoped(typeof(UserFixture));
            //services.AddScoped<IUserFixture, UserFixture>();
        }

        private void ConfigureMocks(IServiceCollection services) { }

    }
}
