using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Template.DataAccess.Database;

var builder = WebApplication.CreateBuilder(args);

var config = builder.Configuration;
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

// Add services to the container.
builder.Services
        .AddDbContext<Context>(options =>
            options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

var identityOptions = new IdentityOptions();
config.GetSection(nameof(IdentityOptions)).Bind(identityOptions);

builder.Services.AddDefaultIdentity<IdentityUser>()
                .AddEntityFrameworkStores<Context>();

builder.Services.AddControllersWithViews();

//builder.Services.Configure<IdentityOptions>(config.GetSection(nameof(IdentityOptions)));
builder.Services.Configure<IdentityOptions>(options =>
{
    options.SignIn.RequireConfirmedAccount = true;
    options.SignIn.RequireConfirmedEmail = true;

    // Password settings.
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 3;
    options.Password.RequiredUniqueChars = 1;

    // Lockout settings.
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    // User settings.
    options.User.AllowedUserNameCharacters =
    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = false;
});

builder.Services.AddAuthentication()
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


