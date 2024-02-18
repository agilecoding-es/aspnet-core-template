using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Template.Application.Contracts;
using Template.Application.Features.LoggingContext.Contracts;
using Template.Application.Features.SampleContext.Contracts;
using Template.Persistence.Database.Interceptors;
using Template.Persistence.SqlServer;
using Template.Persistence.SqlServer.Database;
using Template.Persistence.SqlServer.Respositories.Logging;
using Template.Persistence.SqlServer.Respositories.Sample;

namespace Template.Configuration.Setup
{
    public static class IAppBuilderExtensionsSqlServerSetup
    {
        public static IAppBuilder AddSqlServer(this IAppBuilder appBuilder, string connectionString, bool healthChecksEnabed = true)
        {
            var settings = appBuilder.Configuration.Get<AppSettings>();

            appBuilder.Services.AddDbContext<Context>((sp, options) =>
            {
                options.UseSqlServer(connectionString)
                       .AddInterceptors(new[]
                       {
                           new PublishDomainEventsInterceptor(sp.GetRequiredService<IPublisher>())
                       });
            });

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

    public static class IServiceCollectionExtensionsSqlServerSetup
    {
        public static IServiceCollection AddSqlServer(this IServiceCollection services, string connectionString)
        {
            //services.AddDbContextFactory<Context>(options => options.UseSqlServer(connectionString), lifetime: ServiceLifetime.Scoped);
            services.AddDbContext<Context>(options => options.UseSqlServer(connectionString));

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
