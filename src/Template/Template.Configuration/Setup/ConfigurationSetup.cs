using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template.Configuration.Setup
{
    public static class ConfigurationSetup
    {
        public static IAppBuilder AddSettings(this IAppBuilder appBuilder)
        {
            appBuilder.Services
                        .Configure<AppSettings>(appBuilder.Configuration)
                        .Configure<LoggingExceptionsOptions>(options =>
                        {
                            appBuilder.Configuration.GetSection(LoggingExceptionsOptions.Key).Bind(options);
                        });


            appBuilder.Services.AddSingleton(appBuilder.Configuration.Get<AppSettings>());
            appBuilder.Services.AddSingleton(appBuilder.Configuration.GetSection(LoggingExceptionsOptions.Key).Get<LoggingExceptionsOptions>());

            return appBuilder;
        }
    }
}
