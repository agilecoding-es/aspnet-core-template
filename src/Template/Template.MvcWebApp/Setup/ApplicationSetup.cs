using Microsoft.AspNetCore.Mvc.Razor;
using Serilog;
using Template.Configuration;

namespace Template.MvcWebApp.Setup
{
    public static class ApplicationSetup
    {
        public static AppBuilder CreateAppBuilder(this WebApplicationBuilder builder) => new AppBuilder(builder);

        public static AppBuilder DefaultServicesConfiguration(this WebApplicationBuilder builder)
        {
            var appBuilder = 
                CreateAppBuilder(builder)
                .ConfigureSettings()
                .ConfigureDB()
                .ConfigureIdentity();

            builder.Services.AddControllersWithViews()
                            .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
                            .AddDataAnnotationsLocalization(options =>
                            {
                                options.DataAnnotationLocalizerProvider = (type, factory) =>
                                    factory.Create(Constants.Configuration.Resources.DATANNOTATION, PresentationAssembly.AssemblyName);
                            });

            appBuilder
                .ConfigureDependencies()
                .ConfigureAuthentication()
                .ConfigureAuthorization()
                .ConfigureResources()
                .ConfigureCache()
                .ConfigureMediatr()
                .ConfigureMapster()
                .ConfigureHelthChecks();

            builder.Host.UseSerilog((context, configuration) =>
                configuration.ReadFrom.Configuration(context.Configuration)
            );

            return appBuilder;
        }

    }
}
