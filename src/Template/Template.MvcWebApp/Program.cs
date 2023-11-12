using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NLog;
using NLog.Web;
using Template.Common;
using Template.Configuration;
using Template.MvcWebApp.Middlewares;
using Template.MvcWebApp.Setup;
using Template.Persistence.Database;

// Early init of NLog to allow startup and exception logging, before host is built
var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
logger.Debug("init main");

try
{
    logger.Info("************");
    logger.Info("Starting App");
    logger.Info("************");
    var builder = WebApplication.CreateBuilder(args);
    
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

        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }

#if !DEBUG
    app.UseHttpsRedirection();
#endif
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

    app.MapHealthChecksUI();
    if (settings.HealthChecks.Enabled)
    {
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