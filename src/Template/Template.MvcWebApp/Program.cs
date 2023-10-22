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


var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();

try
{
    // Early init of NLog to allow startup and exception logging, before host is built

    logger.Info("************");
    logger.Info("Initializing App");
    logger.Info("************");
    var builder = WebApplication.CreateBuilder(args);

var config = builder.Configuration;
var connectionString = builder.Configuration.GetConnectionString(Constants.Configuration.ConnectionString.DefaultConnection) ?? throw new InvalidOperationException($"Connection string '{Constants.Configuration.ConnectionString.DefaultConnection}' not found.");

var app = builder.DefaultServicesConfiguration().Build();


    await app.InitializeAsync(config);

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

    app.UseHttpsRedirection();
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
    if (settings.HealthChecksEnabled)
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