﻿using Microsoft.AspNetCore.Mvc.Razor;
using NLog.Web;
using Template.Common;

namespace Template.MvcWebApp.Setup
{
    public static class ApplicationSetup
    {
        public static AppBuilder CreateAppBuilder(this WebApplicationBuilder builder) => new AppBuilder(builder);

        public static AppBuilder DefaultServicesConfiguration(this WebApplicationBuilder builder)
        {
            // NLog: Setup NLog for Dependency injection
            builder.Logging.ClearProviders();
            builder.Host.UseNLog();

            var appBuilder =
                CreateAppBuilder(builder)
                .ConfigureSettings()
                .ConfigureDB()
                .ConfigureIdentity();

            builder.Services.AddHttpContextAccessor();

            builder.Services.AddControllersWithViews()
                            .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
                            .AddDataAnnotationsLocalization(options =>
                            {
                                options.DataAnnotationLocalizerProvider = (type, factory) =>
                                    factory.Create(Constants.Configuration.Resources.DataAnnotation, PresentationAssembly.AssemblyFullName);
                            });

            builder.Services.AddSession();

            appBuilder
                .ConfigureDependencies()
                .ConfigureAuthentication()
                .ConfigureAuthorization()
                .ConfigureResources()
                .ConfigureCache()
                .ConfigureMediatr()
                .ConfigureMapster()
                .ConfigureHelthChecks();


            return appBuilder;
        }

    }
}
