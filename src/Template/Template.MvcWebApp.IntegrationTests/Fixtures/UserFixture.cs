using Bogus;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Template.Domain.Entities.Identity;
using Template.Application.Identity;

namespace Template.MvcWebApp.IntegrationTests.Fixtures
{
    public class UserFixture //: IUserFixture
    {
        private readonly WebAppFactory factory;

        public UserFixture()
        {
            factory = WebAppFactory.FactoryInstance;
        }

        public async Task SerFixtureAsync(int countOfUsers = 10)
        {
            for (int i = 0; i < countOfUsers; i++)
            {
                User user = Activator.CreateInstance<User>();
                var person = new Person();

                if (i < countOfUsers - 1)
                    user.EmailConfirmed = true;

                await factory.ExecuteInScopeAsync<IUserStore<User>>(async userStore =>
                {
                    var emailStore = (IUserEmailStore<User>)userStore;
                    await userStore.SetUserNameAsync(user, $"user{(i + 1).ToString().PadLeft(2, '0')}", CancellationToken.None);
                    await emailStore.SetEmailAsync(user, person.Email, CancellationToken.None);
                });
                await factory.ExecuteInScopeAsync<UserManager>(async userManager =>
                {
                    await userManager.CreateAsync(user, "123");
                });

            }

        }
    }
}
