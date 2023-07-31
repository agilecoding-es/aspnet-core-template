using Microsoft.AspNetCore.Identity;
using System.Reflection;
using Template.Application.Identity;
using Template.Configuration;
using Template.Domain.Entities.Identity;
using Template.Security.Authorization;

namespace Template.MvcWebApp.Setup
{
    public static class AppInitializer
    {
        public static async Task<WebApplication> InitializeAsync(this WebApplication app, ConfigurationManager configuration)
        {
            await ConfigureRolesAsync(app, configuration);
            await ConfigureSuperadminAsync(app, configuration);

            return app;
        }

        private static async Task ConfigureRolesAsync(WebApplication app, ConfigurationManager configuration)
        {
            AppSettings settings = configuration.Get<AppSettings>();

            using (var scope = app.Services.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager>();

                var rolesType = typeof(Roles);
                var roleFields = rolesType.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
                foreach (var roleField in roleFields)
                {
                    if (roleField.FieldType == typeof(string) && roleField.IsLiteral)
                    {
                        var value = roleField.GetValue(null)?.ToString();

                        if (!string.IsNullOrEmpty(value) && !value.Contains(","))
                        {
                            Role role = Activator.CreateInstance<Role>();
                            role.Name = value.ToString();
                            await roleManager.CreateAsync(role);
                        }
                    }
                }
            }
        }
        
        private static async Task ConfigureSuperadminAsync(WebApplication app, ConfigurationManager configuration)
        {
            AppSettings settings = configuration.Get<AppSettings>();

            using (var scope = app.Services.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager>();
                var userStore = scope.ServiceProvider.GetRequiredService<IUserStore<User>>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager>();
                var roleStore = scope.ServiceProvider.GetRequiredService<IRoleStore<Role>>();

                User user = Activator.CreateInstance<User>();
                user.EmailConfirmed = true;
                var emailStore = (IUserEmailStore<User>)userStore;
                await userStore.SetUserNameAsync(user, $"sa", CancellationToken.None);
                await emailStore.SetEmailAsync(user, "malonejv@gmail.com", CancellationToken.None);

                await userManager.CreateAsync(user, settings.SuperadminPass);
                await userManager.AddToRoleAsync(user, Roles.Superadmin);
            }
        }
    }
}
