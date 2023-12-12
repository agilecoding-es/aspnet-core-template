using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Template.Application.Contracts;
using Template.Application.Features.Sample.Contracts;
using Template.Persistence.PosgreSql;
using Template.Persistence.PosgreSql.Database;
using Template.Persistence.PosgreSql.Respositories.Sample;

namespace Template.Configuration.Setup
{
    public static class IAppBuilderExtensionsPostgreSqlSetup
    {
        public static IAppBuilder AddPostgreSql(this IAppBuilder appBuilder, string connectionString)
        {
            appBuilder.Services.AddDbContext<Context>(options => options.UseNpgsql(connectionString));

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


    public static class IServiceCollectionExtensionsPostgreSqlSetup
    {
        public static IServiceCollection AddPostgreSql(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<Context>(options => options.UseNpgsql(connectionString));

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
