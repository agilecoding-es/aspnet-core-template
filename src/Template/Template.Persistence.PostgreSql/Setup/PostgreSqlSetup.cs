using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Npgsql;
using Template.Application.Contracts;
using Template.Application.Features.LoggingContext.Contracts;
using Template.Application.Features.SampleContext.Contracts;
using Template.Persistence.PosgreSql;
using Template.Persistence.PosgreSql.Database;
using Template.Persistence.PosgreSql.Respositories.Logging;
using Template.Persistence.PosgreSql.Respositories.Sample;

namespace Template.Configuration.Setup
{
    public static class IAppBuilderExtensionsPostgreSqlSetup
    {
        public static IAppBuilder AddPostgreSql(this IAppBuilder appBuilder, string connectionString)
        {
            var settings = appBuilder.Configuration.Get<AppSettings>();

            appBuilder.Services.AddDbContext<Context>(
                options => options.UseNpgsql(connectionString)
                                                               .UseSnakeCaseNamingConvention());

            if (settings.HealthChecks.Enabled)
                appBuilder.Services.AddHealthChecksUI().AddPostgreSqlStorage(settings.ConnectionStrings.HealthChecksConnection);

            if (appBuilder.Environment.IsDevelopment())
                appBuilder.Services.AddDatabaseDeveloperPageExceptionFilter();

            appBuilder.Services
                .AddTransient<IUnitOfWork, UnitOfWork>()
                .AddTransient<IExceptionQueryRepository, ExceptionQueryRepository>()
                .AddTransient<ISampleItemRepository, SampleItemRepository>()
                .AddTransient<ISampleItemQueryRepository, SampleItemQueryRepository>()
                .AddTransient<ISampleListRepository, SampleListRepository>()
                .AddTransient<ISampleListQueryRepository, SampleListQueryRepository>();

            return appBuilder;
        }
    }

    public static class IServiceCollectionExtensionsPostgreSqlSetup
    {
        public static IServiceCollection AddPostgreSql(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<Context>(
                options => options.UseNpgsql(connectionString)
                                                               .UseSnakeCaseNamingConvention());

            services
                .AddTransient<IUnitOfWork, UnitOfWork>()
                .AddTransient<IExceptionQueryRepository, ExceptionQueryRepository>()
                .AddTransient<ISampleItemRepository, SampleItemRepository>()
                .AddTransient<ISampleItemQueryRepository, SampleItemQueryRepository>()
                .AddTransient<ISampleListRepository, SampleListRepository>()
                .AddTransient<ISampleListQueryRepository, SampleListQueryRepository>();

            return services;
        }


    }
}
