using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Respawn;
using Respawn.Graph;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template.Configuration;
using Template.MvcWebApp.IntegrationTests.Attributes;
using Template.Persistence.Database;
using static Template.Configuration.Constants.Configuration;

namespace Template.MvcWebApp.IntegrationTests
{
    public class WebAppFactory : WebApplicationFactory<Program>, IAsyncLifetime
    {
        private DbConnection connection = default!;
        private Respawner respawner = default!;

        private IConfiguration Configuration =>
            new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();

        public HttpClient HttpClient { get; private set; } = default;


        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                
            }).
            UseConfiguration(Configuration);

            builder.UseEnvironment("Development");
        }
        public async Task InitializeAsync()
        {
            //_dbcontainer.startAsync();
            connection = new SqlConnection(Configuration.GetConnectionString(Constants.Configuration.ConnectionString.DEFAULT_CONNECTION));
            HttpClient = CreateClient();
            await connection.OpenAsync();
            await InitializeRespawner();
        }

        private async Task InitializeRespawner() => respawner = await Respawner.CreateAsync(
                        connection,
                        new RespawnerOptions()
                        {
                            DbAdapter = DbAdapter.SqlServer,
                            TablesToIgnore = new Table[]
                            {
                        "__EFMigrationsHistory"
                            }
                        });

        public async Task ResetDatabase()
        {
            await respawner.ResetAsync(connection);
        }

        Task IAsyncLifetime.DisposeAsync()
        {
            //_dbcontainer.disposeasync();

            return Task.CompletedTask;
        }
    }

    [CollectionDefinition("WebApp")]
    public class TestCollectionFixture : ICollectionFixture<WebAppFactory> { }
}
