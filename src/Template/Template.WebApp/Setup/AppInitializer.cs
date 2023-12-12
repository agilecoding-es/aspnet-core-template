using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Template.Application.Features.Identity;
using Template.Configuration;
using Template.Configuration.Setup;
using Template.Domain.Entities.Identity;
using Template.Security.Authorization;

namespace Template.WebApp.Setup
{
    public class AppInitializer : IAppInitializer
    {
        public WebApplication App { get; }

        public AppInitializer(WebApplication app)
        {
            this.App = app;
        }

        public async Task<IAppInitializer> ConfigureRolesAsync()
        {

            using (var scope = App.Services.CreateScope())
            {
                var settings = scope.ServiceProvider.GetRequiredService<AppSettings>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager>();
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<RoleManager>>();
                try
                {

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

                                if (!(await roleManager.RoleExistsAsync(role.Name)))
                                    await roleManager.CreateAsync(role);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, $"An error occurred while configuring roles.");
                }
            }

            return this;
        }

        public async Task<IAppInitializer> ConfigureSuperadminAsync()
        {
            using (var scope = App.Services.CreateScope())
            {
                var settings = scope.ServiceProvider.GetRequiredService<AppSettings>();
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<RoleManager>>();

                try
                {
                    var userManager = scope.ServiceProvider.GetRequiredService<UserManager>();
                    var userStore = scope.ServiceProvider.GetRequiredService<IUserStore<User>>();
                    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager>();
                    var roleStore = scope.ServiceProvider.GetRequiredService<IRoleStore<Role>>();

                    var existingSA = await userManager.FindByNameAsync("sa");
                    if (existingSA == null)
                    {
                        User user = Activator.CreateInstance<User>();
                        user.LockoutEnabled = false;
                        user.EmailConfirmed = true;
                        var emailStore = (IUserEmailStore<User>)userStore;
                        await userStore.SetUserNameAsync(user, $"sa", CancellationToken.None);
                        await emailStore.SetEmailAsync(user, "malonejv@gmail.com", CancellationToken.None);

                        await userManager.CreateAsync(user, settings.SuperadminPass);
                        await userManager.AddToRoleAsync(user, Roles.Superadmin);
                    }
                    else
                    {
                        existingSA.LockoutEnd = null;
                        existingSA.LockoutEnabled = false;
                        existingSA.EmailConfirmed = true;
                        existingSA.Email = "malonejv@gmail.com";

                        await userManager.UpdateAsync(existingSA);
                        await userManager.AddToRoleAsync(existingSA, Roles.Superadmin);
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, $"An error occurred while configuring superadmin user.");
                }
            }
            return this;
        }
    }
}
