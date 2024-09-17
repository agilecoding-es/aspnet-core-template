using NLog.Web;
using Template.Application.Features;
using Template.Common;
using Template.Configuration;
using Template.Configuration.Setup;

namespace Template.WebApp.Setup
{
    public static class ApplicationSetup
    {
        public static IAppBuilder CreateAppBuilder(this WebApplicationBuilder builder) => new AppBuilder(builder);

        public static IAppBuilder DefaultServicesConfiguration(this WebApplicationBuilder webApplicationBuilder)
        {
            // NLog: Setup NLog for Dependency injection
            webApplicationBuilder.Logging.ClearProviders();
            webApplicationBuilder.Host.UseNLog();

            webApplicationBuilder.Configuration.AddEnvironmentVariables();

            var connectionString = webApplicationBuilder.Configuration.GetConnectionString(Constants.Configuration.ConnectionString.DefaultConnection) ?? throw new InvalidOperationException($"Connection string '{Constants.Configuration.ConnectionString.DefaultConnection}' not found.");

            var builder =
                CreateAppBuilder(webApplicationBuilder)
                .ConfigureSettings();

#if (UseTemplateConfiguration)

#if (EnableAspNetIdentity)
            bool enableAspNetIdentity = true
#else
            bool enableAspNetIdentity = false;
#endif
            var templateConfiguration = new TemplateConfiguration(enableAspNetIdentity);
#else
            var templateSettings = builder.ConfigureSetting<TemplateOptions>(TemplateOptions.Key);
            var templateConfiguration = new TemplateConfiguration(templateSettings.EnableAspNetIdentity);
#endif

            if (templateConfiguration.EnableAspNetIdentity)
            {
                builder
                    .AddIdentity();
            }

            builder
                .AddPresentation()
                .AddApplicationFeatures()
                .AddPostgreSql(connectionString)
                .AddRedisCacheService()
                .AddMassTransitWithRabbitMQ(ApplicationFeaturesAssembly.Assembly)
                .AddListmonkEmailService()
                .AddHealthChecks();

            if (webApplicationBuilder.Environment.IsStaging())
            {
                builder.AddAzureEmail();
            }
            else
            {
                builder.AddSmtpEmail();
            }

            return builder;
        }

    }
}
