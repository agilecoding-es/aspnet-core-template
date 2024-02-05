using Microsoft.AspNetCore.Identity.UI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template.Infrastructure.EmailService.AzureCommunicationService;
using Template.Infrastructure.EmailService;
using Microsoft.Extensions.DependencyInjection;

namespace Template.Configuration.Setup
{
    public static class AzureEmailSetup
    {
        public static IAppBuilder AddAzureEmail(this IAppBuilder appBuilder)
        {
            appBuilder.Services.AddTransient<IEmailClient, AzureEmailClient>();

            return appBuilder;
        }
    }
}
