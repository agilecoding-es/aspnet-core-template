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
                    .Configure<Mailsettings>(options =>
                    {
                        appBuilder.Configuration.GetSection(nameof(Mailsettings)).Bind(options);
                    })
                    .Configure<LogMiddleware>(options =>
                    {
                        appBuilder.Configuration.GetSection(nameof(LogMiddleware)).Bind(options);
                    });


            var appSettings = appBuilder.Configuration.Get<AppSettings>();
            appBuilder.Services.AddSingleton(appSettings);
            
            var mailSettings = appBuilder.Configuration.Get<Mailsettings>();
            appBuilder.Services.AddSingleton(mailSettings);

            var logMiddlewareSettings = appBuilder.Configuration.Get<LogMiddleware>();
            appBuilder.Services.AddSingleton(logMiddlewareSettings);

            return appBuilder;
        }
    }
}
