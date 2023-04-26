using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Template.Configuration;
using Template.DataAccess.Database;
using Template.MailSender;
using Template.MvcWebApp.Services;

namespace Template.MvcWebApp.Configuration
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureSettings(this IServiceCollection services, ConfigurationManager config)
        {
            services.Configure<AppSettings>(config)
                    .Configure<Mailsettings>(options =>
                    {
                        config.GetSection(nameof(Mailsettings)).Bind(options);
                    });

            return services;
        }

        public static IServiceCollection ConfigureDB(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<Context>(options => options.UseSqlServer(connectionString));

            services.AddDatabaseDeveloperPageExceptionFilter();

            return services;
        }

        public static IServiceCollection ConfigureDependencies(this IServiceCollection services, AppSettings appSettings)
        {
            services
                .AddTransient(provider => new SmtpClient(appSettings.Mailsettings.Host, appSettings.Mailsettings.Port)
                {
                    Credentials = new NetworkCredential(appSettings.Mailsettings.UserName, appSettings.Mailsettings.Password),
                    EnableSsl = appSettings.Mailsettings.EnableSSL
                })
                .AddTransient<ISmtpClientWrapper, SmtpClientWrapper>()
                .AddTransient<IEmailService, EmailSender>()
                .AddTransient<IEmailSender, EmailSender>();

            return services;
        }

    }
}


