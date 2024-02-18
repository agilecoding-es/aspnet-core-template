using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Template.Application.Contracts;
using Template.Application.Features.LoggingContext.Contracts;
using Template.Application.Features.SampleContext.Contracts;
using Template.Persistence.Database.Interceptors;
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

            appBuilder.Services.AddDbContext<Context>((sp, options) =>
                                              options.UseNpgsql(connectionString)
                                                     .AddInterceptors(new[]
                                                     {
                                                         new PublishDomainEventsInterceptor(sp.GetRequiredService<IPublisher>(), sp.GetRequiredService<IPublishEndpoint>())
                                                     })
                                                     .UseSnakeCaseNamingConvention());

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
