using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Template.Configuration;
using Template.DataAccess.Database;
using Template.MvcWebApp.Configuration;

var builder = WebApplication.CreateBuilder(args);

var config = builder.Configuration;
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services
    .ConfigureSettings(config)
    .ConfigureDB(connectionString);

builder.Services
    .AddDefaultIdentity<IdentityUser>()
    .AddEntityFrameworkStores<Context>();

builder.Services.AddControllersWithViews();

builder.Services
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

builder.Services.ConfigureDependencies(config.Get<AppSettings>());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseResponseCaching();
//app.UseResponseCompression();
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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();


