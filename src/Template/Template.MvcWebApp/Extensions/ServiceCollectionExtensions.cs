﻿using System.Globalization;
using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Template.Configuration;
using Template.DataAccess.Database;
using Template.MailSender;
using Template.MvcWebApp.Localization;
using Template.MvcWebApp.Services;
using static Template.Configuration.Constants;

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
        public static IServiceCollection ConfigureIdentity(this IServiceCollection services, ConfigurationManager config)
        {
            services
                .Configure<IdentityOptions>(options =>
                {
                    config.GetSection(nameof(IdentityOptions)).Bind(options);
                })
                .AddAuthentication()
                .AddGoogle(options =>
                {
                    IConfigurationSection googleAuthentication = config.GetSection("Authentication:Google");
                    options.ClientId = googleAuthentication["ClientId"];
                    options.ClientSecret = googleAuthentication["ClientSecret"];
                })
                .AddMicrosoftAccount(microsoftOptions =>
                {
                    IConfigurationSection microsoftAuthentication = config.GetSection("Authentication:Microsoft");
                    microsoftOptions.ClientId = microsoftAuthentication["ClientId"];
                    microsoftOptions.ClientSecret = microsoftAuthentication["ClientSecret"];
                });

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
                .AddTransient<IEmailSender, EmailSender>()
                .AddScoped<ICultureHelper, CultureHelper>()
                .AddSingleton<IViewLocalizer, ViewLocalizer>()
                .AddSingleton<IStringLocalizerFactory, ResourceManagerStringLocalizerFactory>()
                .AddSingleton<IStringLocalizer>(provider =>
                {
                    var factory = provider.GetService<IStringLocalizerFactory>();
                    return factory.Create("AppResources", typeof(Program).Assembly.FullName);
                });


            return services;
        }

        public static IServiceCollection ConfigureResources(this IServiceCollection services, AppSettings appSettings)
        {
            services
                .AddLocalization(options => options.ResourcesPath = "Resources")
                .Configure<RequestLocalizationOptions>(options =>
                {
                    var cookieProvider = new CookieRequestCultureProvider
                    {
                        CookieName = Cookies.CULTURE_COOKIE
                    };
                    var supportedCultures = appSettings.SupportedCultures.GetSupportedCultures().Select(c => new CultureInfo(c)).ToList();

                    options.DefaultRequestCulture = new RequestCulture(appSettings.SupportedCultures.DefaultCulture);
                    options.SupportedCultures = supportedCultures;
                    options.SupportedUICultures = supportedCultures;
                    options.RequestCultureProviders.Insert(0, cookieProvider);
                });

            return services;
        }

    }
}


