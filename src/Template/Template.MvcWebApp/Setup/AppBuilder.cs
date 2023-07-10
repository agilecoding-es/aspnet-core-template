using Mapster;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Localization;
using System.Globalization;
using System.Net;
using System.Net.Mail;
using Template.Application;
using Template.Application.Behaviours;
using Template.Application.Contracts;
using Template.Application.Contracts.Repositories.Sample;
using Template.Application.Identity;
using Template.Configuration;
using Template.Domain.Entities.Identity;
using Template.MailSender;
using Template.MvcWebApp.Enums;
using Template.MvcWebApp.HealthChecks;
using Template.MvcWebApp.Localization;
using Template.MvcWebApp.Services.Rendering;
using Template.Persistence;
using Template.Persistence.Database;
using Template.Persistence.Identity;
using Template.Persistence.Respositories.Sample;
using static Template.Configuration.Constants.Configuration;

namespace Template.MvcWebApp.Setup
{
    public class AppBuilder
    {
        private WebApplicationBuilder builder;
        internal readonly IServiceCollection services;
        internal readonly ConfigurationManager configuration;

        public AppBuilder(WebApplicationBuilder builder)
        {
            this.builder = builder;
            this.services = builder.Services;
            this.configuration = builder.Configuration;
        }

        public AppBuilder AddConfiguration(Action<IServiceCollection, ConfigurationManager> builder)
        {
            _ = builder ?? throw new ArgumentNullException(nameof(builder));

            builder.Invoke(services, configuration);

            return this;
        }

        public AppBuilder ConfigureSettings()
        {
            services.Configure<AppSettings>(configuration)
                    .Configure<Mailsettings>(options =>
                    {
                        configuration.GetSection(nameof(Mailsettings)).Bind(options);
                    });

            return this;
        }

        public AppBuilder ConfigureDB()
        {
            var connectionString = configuration.GetConnectionString(ConnectionString.DEFAULT_CONNECTION) ?? throw new InvalidOperationException($"Connection string '{ConnectionString.DEFAULT_CONNECTION}' not found.");

            //services.AddDbContextFactory<Context>(options => options.UseSqlServer(connectionString), lifetime: ServiceLifetime.Scoped);
            services.AddDbContext<Context>(options => options.UseSqlServer(connectionString));

            services.AddDatabaseDeveloperPageExceptionFilter();

            return this;
        }

        public AppBuilder ConfigureIdentity()
        {
            services.AddDefaultIdentity<User>()
                   .AddUserManager<UserManager>()
                   .AddSignInManager<SignInManager>()
                   .AddUserStore<UserStore>()
                   .AddRoles<Role>()
                   .AddRoleManager<RoleManager>()
                   .AddRoleStore<RoleStore>()
                   .AddEntityFrameworkStores<Context>();

            return this;
        }

        public AppBuilder ConfigureAuthentication()
        {
            services
                .Configure<IdentityOptions>(options =>
                {
                    configuration.GetSection(nameof(IdentityOptions)).Bind(options);
                })
                .AddAuthentication()
                .AddGoogle(options =>
                {
                    IConfigurationSection googleAuthentication = configuration.GetSection("Authentication:Google");
                    options.ClientId = googleAuthentication["ClientId"];
                    options.ClientSecret = googleAuthentication["ClientSecret"];
                })
                .AddMicrosoftAccount(microsoftOptions =>
                {
                    IConfigurationSection microsoftAuthentication = configuration.GetSection("Authentication:Microsoft");
                    microsoftOptions.ClientId = microsoftAuthentication["ClientId"];
                    microsoftOptions.ClientSecret = microsoftAuthentication["ClientSecret"];
                });

            return this;
        }

        public AppBuilder ConfigureAuthorization()
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy(
                    Policies.UserManager.ToString(),
                    policy => policy.RequireRole(Roles.AdminUser.ToString()));

                options.AddPolicy(
                    Policies.RolesManager.ToString(),
                    policy => policy.RequireAssertion(
                        context =>
                        context.User.IsInRole(Roles.AdminUser.ToString()) ||
                        context.User.HasClaim(x => x.Type == Claims.ManageRoles.ToString() && x.Value == bool.TrueString.ToLower())));

            });

            return this;
        }

        public AppBuilder ConfigureDependencies()
        {
            AppSettings appSettings = configuration.Get<AppSettings>();
            
            services.AddTransient<IRazorViewRenderer, RazorViewRenderer>();

            services
                .AddTransient(provider => new SmtpClient(appSettings.Mailsettings.Host, appSettings.Mailsettings.Port)
                {
                    Credentials = new NetworkCredential(appSettings.Mailsettings.UserName, appSettings.Mailsettings.Password),
                    EnableSsl = appSettings.Mailsettings.EnableSSL
                })
                .AddTransient<ISmtpClientWrapper, SmtpClientWrapper>()
                .AddTransient<IEmailService, EmailSender>()
                .AddTransient<IEmailSender, EmailSender>();

            services.TryAddScoped(typeof(IHtmlLocalizer<>), typeof(Localizer<>));

            services
                .AddScoped<ICultureHelper, CultureHelper>()
                .AddSingleton<IHtmlLocalizer, Localizer>()
                .AddSingleton<IViewLocalizer, ViewLocalizer>()
                .AddSingleton<IStringLocalizerFactory, ResourceManagerStringLocalizerFactory>()
                .AddSingleton(provider =>
                {
                    var factory = provider.GetService<IStringLocalizerFactory>()!;
                    return factory.Create(Resources.DEFAULT, PresentationAssembly.AssemblyName);
                });

            services
                .AddTransient<IUnitOfWork, UnitOfWork>()
                .AddTransient<ISampleItemRepository, SampleItemRepository>()
                .AddTransient<ISampleItemQueryRepository, SampleItemQueryRepository>()
                .AddTransient<ISampleListRepository, SampleListRepository>()
                .AddTransient<ISampleListQueryRepository, SampleListQueryRepository>();

            return this;
        }

        public AppBuilder ConfigureResources()
        {
            AppSettings appSettings = configuration.Get<AppSettings>();

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

            return this;
        }

        public AppBuilder ConfigureCache()
        {
            services.AddMemoryCache();

            return this;
        }

        public AppBuilder ConfigureMediatr()
        {
            services.AddMediatR(configuration =>
                    configuration.RegisterServicesFromAssembly(ApplicationAssembly.Assembly))
                .AddTransient(typeof(IPipelineBehavior<,>), typeof(LogginBehavior<,>))
                .AddTransient(typeof(IPipelineBehavior<,>), typeof(CachingBehavior<,>));

            return this;
        }

        public AppBuilder ConfigureMapster()
        {
            TypeAdapterConfig.GlobalSettings.Scan(new[] { PresentationAssembly.Assembly, ApplicationAssembly.Assembly });

            return this;
        }

        public AppBuilder ConfigureHelthChecks()
        {
            services.AddSingleton<LatencyHealthCheck>();
            //services.AddSingleton<IConnectionMultiplexer>(_=> ConnectionMultiplexer.Connect(redisSettings.ConnectionString));

            services.AddHealthChecks()
                    .AddCheck<LatencyHealthCheck>("CustomHealthCheck", tags: new[] { "mvc" })
                    //.AddCheck<RedisHelthCheck>("Redis")
                    .AddCheck("MvcApp", () =>
                        HealthCheckResult.Healthy("App is working as expected."),
                        new[] { "mvc" }
                    )
                    .AddDbContextCheck<Context>("Database", tags:
                        new[] { "database", "sql server" }
                    );

            services.AddHealthChecksUI().AddInMemoryStorage();

            return this;
        }

        public WebApplication Build() => builder.Build();

    }
}
