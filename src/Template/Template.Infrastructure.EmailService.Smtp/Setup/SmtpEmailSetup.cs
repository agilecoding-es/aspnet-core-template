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
                        .Configure<EmailServiceOptions>(options =>
                        {
                            appBuilder.Configuration.GetSection(EmailServiceOptions.Key).Bind(options);
                        });


            var emailServiceOptions = appBuilder.Configuration.GetSection(EmailServiceOptions.Key).Get<EmailServiceOptions>();
            appBuilder.Services.AddSingleton(emailServiceOptions);
            appBuilder.Services.AddTransient<IEmailClient, SmtpEmailClient>();

            return appBuilder;
        }
    }
}
