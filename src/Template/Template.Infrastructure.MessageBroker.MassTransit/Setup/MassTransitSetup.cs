using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Template.Infrastructure.MessageBroker.MassTransit.Settings;

namespace Template.Configuration.Setup
{
    public static class IAppBuilderExtensionsMassTransitSetup
    {
        public static IAppBuilder AddMassTransitWithRabbitMQ(this IAppBuilder appBuilder, params Assembly[] consumers)
        {
            var settings = appBuilder.Configuration.Get<AppSettings>();

            appBuilder.Services.AddMassTransit(busConfigurator =>
            {
                busConfigurator.SetKebabCaseEndpointNameFormatter();
                busConfigurator.AddConsumers(consumers);
                busConfigurator.UsingRabbitMq((context, configurator) =>
                {
                    MessageBrokerServiceOptions options = context.GetRequiredService<MessageBrokerServiceOptions>();
                    configurator.Host(new Uri(options.Host), h =>
                    {
                        h.Username(options.Username);
                        h.Password(options.Password);
                    });
                    configurator.ConfigureEndpoints(context);
                });
            });

            appBuilder.Services
                        .Configure<MessageBrokerServiceOptions>(options =>
                        {
                            appBuilder.Configuration.GetSection(MessageBrokerServiceOptions.Key).Bind(options);
                        });
            appBuilder.Services.AddSingleton(appBuilder.Configuration.GetSection(MessageBrokerServiceOptions.Key).Get<MessageBrokerServiceOptions>());

            return appBuilder;
        }
    }

    //public static class IServiceCollectionExtensionsSqlServerSetup
    //{
    //    public static IServiceCollection AddSqlServer(this IServiceCollection services, string connectionString)
    //    {
    //        //services.AddDbContextFactory<Context>(options => options.UseSqlServer(connectionString), lifetime: ServiceLifetime.Scoped);
    //        services.AddDbContext<Context>(options => options.UseSqlServer(connectionString));

    //        services
    //            .AddTransient<IUnitOfWork, UnitOfWork>()
    //            .AddTransient<IExceptionQueryRepository, ExceptionQueryRepository>()
    //            .AddTransient<ISampleItemRepository, SampleItemRepository>()
    //            .AddTransient<ISampleItemQueryRepository, SampleItemQueryRepository>()
    //            .AddTransient<ISampleListRepository, SampleListRepository>()
    //            .AddTransient<ISampleListQueryRepository, SampleListQueryRepository>();

    //        return services;
    //    }
    //}
}
