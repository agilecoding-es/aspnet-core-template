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
                    .Configure<AzureEmailServiceOptions>(options =>
                    {
                        appBuilder.Configuration.GetSection(nameof(AzureEmailServiceOptions)).Bind(options);
                    });


            var azureEmailServiceOptions = appBuilder.Configuration.Get<AzureEmailServiceOptions>();
            appBuilder.Services.AddSingleton(azureEmailServiceOptions);

            appBuilder.Services.AddTransient<IEmailClient, AzureEmailClient>();

            return appBuilder;
        }
    }
}
