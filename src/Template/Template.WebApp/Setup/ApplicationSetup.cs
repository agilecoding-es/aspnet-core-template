﻿using NLog.Web;
using Template.Common;
using Template.Configuration.Setup;

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
                .AddPostgreSql(connectionString)
                .AddListmonkMailService()
                .AddIdentity()
                .AddPresentation()
                .AddApplicationFeatures()
                .AddHelthChecks();

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
