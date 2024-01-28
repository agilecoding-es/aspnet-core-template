using Microsoft.AspNetCore.Identity.UI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template.Infrastructure.Mails.Smtp;
using Template.Infrastructure.Mails;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using System.Net;

namespace Template.Configuration.Setup
{
    public static class SmtpEmailSetup
    {
        public static IAppBuilder AddSmtpEmail(this IAppBuilder appBuilder)
        {
            var mailSettings = appBuilder.Configuration.Get<IntegratedMailOptions>();

            appBuilder.Services
                .AddTransient(provider => new SmtpClient(mailSettings.Host, mailSettings.Port)
                {
                    Credentials = new NetworkCredential(mailSettings.UserName, mailSettings.Password),
                    EnableSsl = mailSettings.EnableSSL
                });

            appBuilder.Services.AddTransient<IEmailClient, SmtpEmailClient>();

            return appBuilder;
        }
    }
}
