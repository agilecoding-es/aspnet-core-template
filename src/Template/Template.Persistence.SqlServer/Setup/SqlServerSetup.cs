using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Template.Application.Contracts;
using Template.Application.Features.Sample.Contracts;
using Template.Persistence.SqlServer;
using Template.Persistence.SqlServer.Database;
using Template.Persistence.SqlServer.Respositories.Sample;

namespace Template.Configuration.Setup
{
    public static class IAppBuilderExtensionsSqlServerSetup
    {
        public static IAppBuilder AddSqlServer(this IAppBuilder appBuilder, string connectionString)
        {
            //services.AddDbContextFactory<Context>(options => options.UseSqlServer(connectionString), lifetime: ServiceLifetime.Scoped);
            appBuilder.Services.AddDbContext<Context>(options => options.UseSqlServer(connectionString));

            if (appBuilder.Environment.IsDevelopment())
            {
                appBuilder.Services.AddDatabaseDeveloperPageExceptionFilter();
            }

            appBuilder.Services
                .AddTransient<IUnitOfWork, UnitOfWork>()
                .AddTransient<ISampleItemRepository, SampleItemRepository>()
                .AddTransient<ISampleItemQueryRepository, SampleItemQueryRepository>()
                .AddTransient<ISampleListRepository, SampleListRepository>()
                .AddTransient<ISampleListQueryRepository, SampleListQueryRepository>();

            return appBuilder;
        }
    }
    public static class IServiceCollectionExtensionsSqlServerSetup
    {
        public static IServiceCollection AddSqlServer(this IServiceCollection services, string connectionString)
        {
            //services.AddDbContextFactory<Context>(options => options.UseSqlServer(connectionString), lifetime: ServiceLifetime.Scoped);
            services.AddDbContext<Context>(options => options.UseSqlServer(connectionString));

            services
                .AddTransient<IUnitOfWork, UnitOfWork>()
                .AddTransient<ISampleItemRepository, SampleItemRepository>()
                .AddTransient<ISampleItemQueryRepository, SampleItemQueryRepository>()
                .AddTransient<ISampleListRepository, SampleListRepository>()
                .AddTransient<ISampleListQueryRepository, SampleListQueryRepository>();

            return services;
        }
    }
}
