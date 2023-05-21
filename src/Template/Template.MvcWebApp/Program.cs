using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Serilog;
using Template.Application.Identity;
using Template.Configuration;
using Template.Domain.Entities.Identity;
using Template.MvcWebApp.Configuration;
using Template.Persistence.Database;
using Template.Persistence.Identity;

var builder = WebApplication.CreateBuilder(args);

var config = builder.Configuration;
var connectionString = builder.Configuration.GetConnectionString(Constants.Configuration.ConnectionString.DEFAULT_CONNECTION) ?? throw new InvalidOperationException($"Connection string '{Constants.Configuration.ConnectionString.DEFAULT_CONNECTION}' not found.");

builder.Services
    .ConfigureSettings(config)
    .ConfigureDB(connectionString);

builder.Services
       .AddDefaultIdentity<User>()
       .AddUserManager<UserManager>()
       .AddSignInManager<SignInManager>()
       .AddUserStore<UserStore>()
       .AddEntityFrameworkStores<Context>();

builder.Services.AddControllersWithViews()
                .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
                .AddDataAnnotationsLocalization(options =>
                {
                    options.DataAnnotationLocalizerProvider = (type, factory) =>
                        factory.Create(Constants.Configuration.Resources.DATANNOTATION, typeof(Program).Assembly.FullName);
                });

builder.Services.ConfigureIdentity(config)
                .ConfigureDependencies(config.Get<AppSettings>())
                .ConfigureResources(config.Get<AppSettings>())
                .ConfigureCache()
                .ConfigureMediatr()
                .ConfigureMapster()
                .ConfigureHelthChecks();

builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration)
);

var app = builder.Build();
var locOptions = app.Services.GetService<IOptions<RequestLocalizationOptions>>();

//Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error")
                   .UseWhen(context => !context.Request.Path.StartsWithSegments("/api"),
                   appBuilder => appBuilder.UseStatusCodePagesWithReExecute("/Home/error/{0}"));

    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseSerilogRequestLogging();

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

app.MapRazorPages();

app.Run();


