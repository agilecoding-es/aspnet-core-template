using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Mail;
using Template.Infrastructure.EmailService;
using Template.Infrastructure.EmailService.Smtp;
using Template.Infrastructure.EmailService.Smtp.Settings;

namespace Template.Configuration.Setup
{
    public static class SmtpEmailSetup
    {
        public static IAppBuilder AddSmtpEmail(this IAppBuilder appBuilder)
        {
            appBuilder.Services
                        .Configure<MailSettingOptions>(options =>
                        {
                            appBuilder.Configuration.GetSection(MailSettingOptions.Key).Bind(options);
                        });


            var mailSettings = appBuilder.Configuration.GetSection(MailSettingOptions.Key).Get<MailSettingOptions>();
            appBuilder.Services.AddSingleton(mailSettings);
            appBuilder.Services.AddTransient<IEmailClient, SmtpEmailClient>();

            return appBuilder;
        }
    }
}
