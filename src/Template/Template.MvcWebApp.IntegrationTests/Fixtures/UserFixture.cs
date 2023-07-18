using Bogus;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Template.Domain.Entities.Identity;
using Template.Application.Identity;
using Microsoft.AspNetCore.Builder;
using System.Reflection;
using Template.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Template.Security.Authorization;

namespace Template.MvcWebApp.IntegrationTests.Fixtures
{
    public class UserFixture //: IUserFixture
    {
        private readonly WebAppFactory factory;

        public UserFixture()
        {
            factory = WebAppFactory.FactoryInstance;
        }

        /// <summary>
        /// Sets a superadmin user, an admin user and the count of site users of parameter <paramref name="countOfUsers"/> indicates.
        /// </summary>
        /// <param name="countOfUsers">Count of normal users</param>
        /// <returns></returns>
        public async Task SerFixtureAsync(int countOfUsers = 5)
        {
            await ConfigureRolesAsync();
            await ConfigureSuperadminAsync();

            User user = Activator.CreateInstance<User>();
            var person = new Person();
            user.EmailConfirmed = true;

            await factory.ExecuteInScopeAsync<IUserStore<User>>(async userStore =>
            {
                var emailStore = (IUserEmailStore<User>)userStore;
                await userStore.SetUserNameAsync(user, $"admin", CancellationToken.None);
                await emailStore.SetEmailAsync(user, "admin@test.com", CancellationToken.None);
            });
            await factory.ExecuteInScopeAsync<UserManager>(async userManager =>
            {
                await userManager.CreateAsync(user, "123");
                await userManager.AddToRoleAsync(user, Roles.Admin);
            });


            for (int i = 0; i < countOfUsers; i++)
            {
                user = Activator.CreateInstance<User>();
                person = new Person();

                if (i < countOfUsers - 1)
                    user.EmailConfirmed = true;

                await factory.ExecuteInScopeAsync<IUserStore<User>>(async userStore =>
                {
                    var emailStore = (IUserEmailStore<User>)userStore;
                    await userStore.SetUserNameAsync(user, $"user{(i + 1)}", CancellationToken.None);
                    await emailStore.SetEmailAsync(user, person.Email, CancellationToken.None);
                });
                await factory.ExecuteInScopeAsync<UserManager>(async userManager =>
                {
                    await userManager.CreateAsync(user, "123");
                    await userManager.AddToRoleAsync(user, Roles.User);
                });
            }

        }


        private async Task ConfigureRolesAsync()
        {
            await factory.ExecuteInScopeAsync(async services =>
            {
                var roleManager = services.GetService<RoleManager>();

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
            });
        }

        private async Task ConfigureSuperadminAsync()
        {
            AppSettings settings = WebAppFactory.FactoryConfiguration.Settings;

            await factory.ExecuteInScopeAsync(async services =>
            {
                var userManager = services.GetService<UserManager>();
                var userStore = services.GetService<IUserStore<User>>();
                var roleManager = services.GetService<RoleManager>();
                var roleStore = services.GetService<IRoleStore<Role>>();

                User user = Activator.CreateInstance<User>();
                user.EmailConfirmed = true;
                var emailStore = (IUserEmailStore<User>)userStore;
                await userStore.SetUserNameAsync(user, $"sa", CancellationToken.None);
                await emailStore.SetEmailAsync(user, "malonejv@gmail.com", CancellationToken.None);

                await userManager.CreateAsync(user, settings.SuperadminPass);
                await userManager.AddToRoleAsync(user, Roles.Superadmin);
            });
        }
    }
}
