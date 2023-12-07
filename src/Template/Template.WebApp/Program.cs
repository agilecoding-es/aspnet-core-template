using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Options;
using NLog;
using NLog.Web;
using System.Security.Cryptography.X509Certificates;
using Template.Configuration;
using Template.WebApp.Middlewares;
using Template.WebApp.Setup;
using Template.Persistence.SqlServer.Database;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using HealthChecks.UI.Client;

var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
try
{
    var builder = WebApplication.CreateBuilder(args);

    // Early init of NLog to allow startup and exception logging, before host is built
    logger.Info("************");
    logger.Info("Starting App");
    logger.Info("************");

    if (builder.Environment.IsProduction())
    {
        builder.Services.Configure<ForwardedHeadersOptions>(options =>
        {
            options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
        });
        builder.Services.AddCertificateForwarding(options =>
        {
            options.CertificateHeader = "X-SSL-CERTIFICATE";
            options.HeaderConverter = (headerValue) =>
            {
                //// Conversion logic to create an X509Certificate2.
                //var clientCertificate = ConversionLogic.CreateAnX509Certificate2();
                //return clientCertificate;
                try
                {
                    // Convierte el valor del encabezado a un arreglo de bytes
                    byte[] certificateBytes = Convert.FromBase64String(headerValue);

                    // Crea un objeto X509Certificate2 a partir del arreglo de bytes
                    X509Certificate2 clientCertificate = new X509Certificate2(certificateBytes);

                    // Devuelve el objeto X509Certificate2
                    return clientCertificate;
                }
                catch (Exception ex)
                {
                    // Manejar cualquier excepción que pueda ocurrir durante la conversión
                    Console.WriteLine($"Error al convertir el certificado: {ex.Message}");
                    return null; // O maneja de otra manera, según tus requisitos
                }
            };
        });
    }
    builder.Services.AddHttpLogging(options =>
    {
        options.LoggingFields = HttpLoggingFields.RequestPropertiesAndHeaders;
    });


    var app = builder.DefaultServicesConfiguration().Build();

    logger.Info("App Initialization \t | Initializing app ...");
    logger.Info("- Applying migrations ");
    logger.Info("- Configuring roles ");
    logger.Info("- Configuring superadmin ");
    await app.InitializeAsync<Context>(builder.Configuration);
    logger.Info("App Initialization \t | App initialized!");

    var settings = app.Configuration.Get<AppSettings>();

    //----------------------------------------------
    //Configure the HTTP request pipeline.
    //----------------------------------------------
    var locOptions = app.Services.GetService<IOptions<RequestLocalizationOptions>>();

    _ = locOptions ?? throw new ArgumentException(nameof(RequestLocalizationOptions));


    if (app.Environment.IsDevelopment())
    {
        app.UseMigrationsEndPoint();
        app.UseDeveloperExceptionPage();
    }
    else
    {
        app.UseLogExceptions()
           .UseExceptionHandler("/Error/500")
                       .UseWhen(context => !context.Request.Path.StartsWithSegments("/api"),
                       appBuilder => appBuilder.UseStatusCodePagesWithReExecute("/Error/{0}"));

        if (app.Environment.IsProduction())
        {
            app.Use((context, next) =>
            {
                context.Request.Scheme = "https";
                return next(context);
            });
            app.UseCertificateForwarding();
            app.UseForwardedHeaders();
            if (settings.LogHttpEnabled)
            {
                app.UseHttpLogging();
                app.Use(async (context, next) =>
                {
                    // Connection: RemoteIp
                    app.Logger.LogInformation("Request RemoteIp: {RemoteIpAddress}", context.Connection.RemoteIpAddress);

                    await next(context);
                });
            }
        }

        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }

    //app.UseHttpsRedirection();
    app.UseResponseCaching();
    //app.UseResponseCompression();

    app.UseRequestLocalization(locOptions.Value);
    app.UseStaticFiles();
    app.UseCookiePolicy();

    app.UseRouting();
    // app.UseRequestLocalization();
    // app.UseCors();

    app.UseAuthentication();
    app.UseAuthorization();
    // app.UseSession();
    // app.UseResponseCompression();
    // app.UseResponseCaching();

    app.MapControllerRoute("default-for-areas", "{area:exists}/{controller=Home}/{action=Index}/{id?}");
    app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");

    if (settings.HealthChecks.Enabled)
    {
        app.MapHealthChecksUI();

        app.MapHealthChecks("/health", new HealthCheckOptions()
        {
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });
        app.MapHealthChecks("/health/databases", new HealthCheckOptions()
        {
            Predicate = registration => registration.Tags.Contains("database"),
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });
        app.MapHealthChecks("/healthoverview", new HealthCheckOptions()
        {
            Predicate = _ => false
        });
    }
    app.MapRazorPages();

    app.Run();

}
catch (Exception exception)
{
    // NLog: catch setup errors
    logger.Error(exception, "Stopped program because of exception");
    throw;
}
finally
{
    // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
    LogManager.Shutdown();
}
