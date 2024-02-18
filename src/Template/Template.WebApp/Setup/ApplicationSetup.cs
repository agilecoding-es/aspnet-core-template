using MassTransit;
using MassTransit.Configuration;
using NLog.Web;
using Template.Application.Contracts.EventBus;
using Template.Application.Features;
using Template.Application.Features.SampleContext.Lists.Events;
using Template.Common;
using Template.Configuration;
using Template.Configuration.Setup;
using Template.Infrastructure.MessageBroker.MassTransit;

namespace Template.WebApp.Setup
{
    public static class ApplicationSetup
    {
        public static IAppBuilder CreateAppBuilder(this WebApplicationBuilder builder) => new AppBuilder(builder);

        public static IAppBuilder DefaultServicesConfiguration(this WebApplicationBuilder builder)
        {
            // NLog: Setup NLog for Dependency injection
            builder.Logging.ClearProviders();
            builder.Host.UseNLog();

            builder.Configuration.AddEnvironmentVariables();

            var connectionString = builder.Configuration.GetConnectionString(Constants.Configuration.ConnectionString.DefaultConnection) ?? throw new InvalidOperationException($"Connection string '{Constants.Configuration.ConnectionString.DefaultConnection}' not found.");

            var appBuilder =
                CreateAppBuilder(builder)
                .AddSettings()
                .AddRedisCacheService()
                .AddListmonkEmailService()
                .AddIdentity()
                .AddPresentation()
                .AddApplicationFeatures()
                .AddPostgreSql(connectionString)
                .AddHealthChecks()
                .AddService(services =>
                {
                    services.AddMassTransit(busConfigurator =>
                    {
                        busConfigurator.SetKebabCaseEndpointNameFormatter();
                        busConfigurator.AddConsumers(ApplicationFeaturesAssembly.Assembly);
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
                        //busConfigurator.UsingInMemory((context, configurator) =>
                        //{
                        //    configurator.ConfigureEndpoints(context);
                        //});
                    });
                    services.AddTransient<IEventBus, EventBus>();
                });

            if (builder.Environment.IsStaging())
            {
                appBuilder.AddAzureEmail();
            }
            else
            {
                appBuilder.AddSmtpEmail();
            }

            return appBuilder;
        }

    }
}
