using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Localization;
using System.Globalization;
using System.Net;
using System.Net.Mail;
using System.Reflection.PortableExecutable;
using Template.Application;
using Template.Application.Behaviours;
using Template.Application.Contracts;
using Template.Application.Features;
using Template.Application.Features.Identity;
using Template.Application.Features.Sample.Contracts;
using Template.Common;
using Template.Common.Extensions;
using Template.Configuration;
using Template.Configuration.Setup;
using Template.Domain.Entities.Identity;
using Template.Infrastructure.Mails;
using Template.Infrastructure.Mails.AzureCommunicationService;
using Template.Infrastructure.Mails.Smtp;
using Template.Persistence.Identity.PosgreSql;
using Template.Persistence.PosgreSql;
using Template.Persistence.PosgreSql.Database;
using Template.Persistence.PosgreSql.Respositories.Sample;
using Template.Security.Authorization;
using Template.Security.Authorization.Requirements;
using Template.WebApp.Localization;
using Template.WebApp.Resources;
using Template.WebApp.Services.Rendering;
using LatencyHealthCheck = Template.WebApp.HealthChecks.LatencyHealthCheck;

namespace Template.WebApp.Setup
{
    public class AppBuilder : IAppBuilder
    {
        private WebApplicationBuilder builder;

        public AppBuilder(WebApplicationBuilder builder)
        {
            this.builder = builder;
            this.Services = builder.Services;
            this.Configuration = builder.Configuration;
            this.Environment = builder.Environment;
        }

        public IServiceCollection Services { get; }
        public ConfigurationManager Configuration { get; }
        public IWebHostEnvironment Environment { get; }

        public IAppBuilder AddSettings(Action<IServiceCollection, ConfigurationManager> builder)
        {
            _ = builder ?? throw new ArgumentNullException(nameof(builder));

            builder.Invoke(Services, Configuration);

            return this;
        }

        public IAppBuilder AddIdentity()
        {
            Services.AddDefaultIdentity<User>()
                   .AddUserManager<UserManager>()
                   .AddSignInManager<SignInManager>()
                   .AddUserStore<UserStore>()
                   .AddRoles<Role>()
                   .AddRoleManager<RoleManager>()
                   .AddRoleStore<RoleStore>()
                   .AddEntityFrameworkStores<Context>();

            return this;
        }

        public IAppBuilder AddPresentation()
        {
            Services
                .AddHttpContextAccessor()
                .AddControllersWithViews()
                    .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
                    .AddDataAnnotationsLocalization(options =>
                            {
                                options.DataAnnotationLocalizerProvider = (type, factory) =>
                                    factory.Create(Constants.Configuration.Resources.DataAnnotation, PresentationAssembly.AssemblyFullName);
                            });

            Services.AddSession();

            this.AddDependencies()
                .AddAuthentication()
                .AddAuthorization()
                .AddResources()
                .AddMapster();

            return this;
        }

        public IAppBuilder AddAuthentication()
        {
            var appSettings = Configuration.Get<AppSettings>();
            var authProviders = appSettings.AuthenticationProviders;
            Services
                .Configure<IdentityOptions>(options =>
                {
                    Configuration.GetSection(nameof(IdentityOptions)).Bind(options);
                })
                .AddAuthentication()
                .AddGoogle(options =>
                {
                    options.ClientId = authProviders.Google.ClientId;
                    options.ClientSecret = authProviders.Google.ClientSecret;
                })
                .AddMicrosoftAccount(options =>
                {
                    options.ClientId = authProviders.Microsoft.ClientId;
                    options.ClientSecret = authProviders.Microsoft.ClientSecret;
                });

            return this;
        }

        public IAppBuilder AddAuthorization()
        {
            Services.AddAuthorization(options =>
            {
                #region User Policies

                options.AddPolicy(
                    Policies.AdminUser,
                    policy => policy.RequireAssertion(
                        context =>
                        context.User.IsInRole(Roles.Superadmin) ||
                        context.User.IsInRole(Roles.Admin)
                ));

                options.AddPolicy(
                    Policies.EditUser,
                    policy => policy.RequireAssertion(
                        context =>
                        context.User.IsInRole(Roles.Superadmin) ||
                        context.User.IsInRole(Roles.Admin) ||
                        context.User.HasClaim(x => x.Type == ApplicationClaimTypes.EditUser && x.Value == true.AsString())
                ));

                options.AddPolicy(
                    Policies.DeleteUser,
                    policy => policy.RequireAssertion(
                        context =>
                        context.User.IsInRole(Roles.Superadmin) ||
                        context.User.IsInRole(Roles.Admin) ||
                        context.User.HasClaim(x => x.Type == ApplicationClaimTypes.DeleteUser && x.Value == true.AsString())
                ));

                #endregion

                #region Role Policies

                options.AddPolicy(
                    Policies.AddRole,
                    policy => policy.RequireAssertion(
                        context =>
                        context.User.IsInRole(Roles.Superadmin) ||
                        context.User.HasClaim(x => x.Type == ApplicationClaimTypes.AddRole && x.Value == true.AsString())
                ));

                options.AddPolicy(
                    Policies.EditRole,
                    policy => policy.RequireAssertion(
                        context =>
                        context.User.IsInRole(Roles.Superadmin) ||
                        context.User.IsInRole(Roles.Admin) ||
                        context.User.HasClaim(x => x.Type == ApplicationClaimTypes.EditRole && x.Value == true.AsString())
                ));

                options.AddPolicy(
                    Policies.DeleteRole,
                    policy => policy.RequireAssertion(
                        context =>
                        context.User.IsInRole(Roles.Superadmin) ||
                        context.User.IsInRole(Roles.Admin) ||
                        context.User.HasClaim(x => x.Type == ApplicationClaimTypes.DeleteRole && x.Value == true.AsString())
                ));


                options.AddPolicy(
                    Policies.EditAdminRole,
                    policy => policy.AddRequirements(new ManageAdminRolesAndClaimsRequirement()));

                #endregion

            });

            return this;
        }

        public IAppBuilder AddDependencies()
        {
            Services
                .AddTransient<IRazorViewRenderer, RazorViewRenderer>()
                .AddTransient<IAuthorizationHandler, CanEditOnlyOtherAdminRolesAndClaimsHandler>()
                .AddScoped<ICultureHelper, CultureHelper>()
                .AddSingleton<IHtmlLocalizer, HtmlLoc>()
                .AddSingleton<IHtmlLocalizer<AppResources>, HtmlLoc<AppResources>>()
                .AddSingleton<IStringLocalizer, StringLoc<AppResources>>()
                .AddSingleton<IViewLocalizer, ViewLocalizer>()
                .AddSingleton<IStringLocalizerFactory, ResourceManagerStringLocalizerFactory>()
                .AddSingleton(provider =>
                {
                    var factory = provider.GetService<IStringLocalizerFactory>()!;
                    return factory.Create(Constants.Configuration.Resources.AppResources, PresentationAssembly.AssemblyFullName);
                });

            return this;
        }

        public IAppBuilder AddResources()
        {
            var appSettings = Configuration.Get<AppSettings>();

            Services
                .AddLocalization(options => options.ResourcesPath = "Resources")
                .Configure<RequestLocalizationOptions>(options =>
                {
                    var cookieProvider = new CookieRequestCultureProvider
                    {
                        CookieName = Constants.Configuration.Cookies.CultureCookieName
                    };
                    var supportedCultures = appSettings.SupportedCultures.GetSupportedCultures().Select(c => new CultureInfo(c)).ToList();

                    options.DefaultRequestCulture = new RequestCulture(appSettings.SupportedCultures.DefaultCulture);
                    options.SupportedCultures = supportedCultures;
                    options.SupportedUICultures = supportedCultures;
                    options.RequestCultureProviders.Insert(0, cookieProvider);
                });

            return this;
        }

        public IAppBuilder AddMapster()
        {
            TypeAdapterConfig.GlobalSettings.Scan(new[] { PresentationAssembly.Assembly, ApplicationAssembly.Assembly });

            return this;
        }

        public IAppBuilder AddHelthChecks()
        {
            var appSettings = Configuration.Get<AppSettings>();

            if (appSettings.HealthChecks.Enabled)
            {
                Services.AddSingleton<LatencyHealthCheck>();
                //services.AddSingleton<IConnectionMultiplexer>(_=> ConnectionMultiplexer.Connect(redisSettings.ConnectionString));

                Services.AddHealthChecks()
                        //.AddCheck<LatencyHealthCheck>("LatencyHealthCheck", tags: new[] { "mvc" })
                        //.AddCheck<RedisHelthCheck>("Redis")
                        .AddCheck("MvcApp", () =>
                            HealthCheckResult.Healthy("App is working as expected."),
                            new[] { "mvc" }
                        )
                        .AddDbContextCheck<Context>("Database", tags:
                            new[] { "database", "sql server" }
                );

                Services.AddHealthChecksUI().AddPostgreSqlStorage(appSettings.ConnectionStrings.DefaultConnection);
                //Services.AddHealthChecksUI().AddSqlServerStorage(appSettings.ConnectionStrings.DefaultConnection);
            }
            return this;
        }

        public WebApplication Build() => builder.Build();

    }
}
