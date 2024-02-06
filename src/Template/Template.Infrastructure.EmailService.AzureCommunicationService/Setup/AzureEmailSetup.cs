using Microsoft.AspNetCore.Identity.UI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template.Infrastructure.EmailService.AzureCommunicationService;
using Template.Infrastructure.EmailService;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Template.Infrastructure.EmailService.AzureCommunicationService.Settings;

namespace Template.Configuration.Setup
{
    public static class AzureEmailSetup
    {
        public static IAppBuilder AddAzureEmail(this IAppBuilder appBuilder)
        {
            appBuilder.Services
                    .Configure<AzureMailSettingOptions>(options =>
                    {
                        appBuilder.Configuration.GetSection(nameof(AzureMailSettingOptions)).Bind(options);
                    });


            var mailSettings = appBuilder.Configuration.Get<AzureMailSettingOptions>();
            appBuilder.Services.AddSingleton(mailSettings);

            appBuilder.Services.AddTransient<IEmailClient, AzureEmailClient>();

            return appBuilder;
        }
    }
}
