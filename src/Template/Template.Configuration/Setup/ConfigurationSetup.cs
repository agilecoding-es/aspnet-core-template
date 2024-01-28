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
            appBuilder.Services.Configure<AppSettings>(appBuilder.Configuration)
                    .Configure<IntegratedMailOptions>(options =>
                    {
                        appBuilder.Configuration.GetSection(nameof(IntegratedMailOptions)).Bind(options);
                    })
                    .Configure<LoggingOptions>(options =>
                    {
                        appBuilder.Configuration.GetSection(nameof(LoggingOptions)).Bind(options);
                    });


            var appSettings = appBuilder.Configuration.Get<AppSettings>();
            appBuilder.Services.AddSingleton(appSettings);
            
            var mailSettings = appBuilder.Configuration.Get<IntegratedMailOptions>();
            appBuilder.Services.AddSingleton(mailSettings);

            var logMiddlewareSettings = appBuilder.Configuration.Get<LoggingOptions>();
            appBuilder.Services.AddSingleton(logMiddlewareSettings);

            return appBuilder;
        }
    }
}
