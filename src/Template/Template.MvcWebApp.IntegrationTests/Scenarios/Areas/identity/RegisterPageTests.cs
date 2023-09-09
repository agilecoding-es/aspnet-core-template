using Acheve.TestHost;
using AngleSharp.Html.Dom;
using Azure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Org.BouncyCastle.Ocsp;
using System.IO;
using System.Net;
using System.Security.Claims;
using System.Text;
using Template.Application.Identity;
using Template.Common.Extensions;
using Template.Domain.Entities.Identity;
using Template.MvcWebApp.IntegrationTests.Attributes;
using Template.MvcWebApp.IntegrationTests.Extensions;
using Template.MvcWebApp.IntegrationTests.Fixtures;
using Template.Security.Authorization;
using static System.Net.WebRequestMethods;

namespace Template.MvcWebApp.IntegrationTests.Scenarios.Areas.identity
{
    [Collection("WebApp")]
    public class RegisterPageTests
    {
        const string AREA = "Identity";
        const string PATH = "Account";
        const string PAGE = "Register";
        private readonly WebAppFactory factory;
        private readonly UserFixture userFixture;

        public RegisterPageTests()
        {
            factory = WebAppFactory.GetFactoryInstance();
            userFixture = new UserFixture(factory);
        }

        [Fact(Skip = "Not working")]
        [CheckExceptions()]
        [ResetDatabase()]
        public async Task WhenUserPostRegister_WithValidParameters_UserIsCreatedAndAssignToRoleUser()
        {
            // Arrange
            var url = $"{AREA}/{PATH}/{PAGE}";
            var username = "testuser";

            await userFixture.SetFixtureAsync();

            var expectedUsersCount = 0;
            var expectedRolesCount = 0;
            await factory.ExecuteInScopeAsync(services =>
            {
                var userManager = services.GetService<UserManager>();
                var roleManager = services.GetService<RoleManager>();

                expectedUsersCount = userManager.Users.Count() + 1;
                expectedRolesCount = roleManager.Roles.Count();

                return Task.CompletedTask;
            });


            var client = factory.Server.CreateClient();

            var model = new Dictionary<string, string>()
            {
                { "Input.UserName" , username},
                { "Input.Email" , $"{username}@sample.com"},
                { "Input.Password" , "123456"},
                { "Input.ConfirmPassword" , "123456" },
            };

            var response = await factory.CreateClient()
                                        .PostAsync(url, model.AsFormUrlEncodedContent());

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("text/html; charset=utf-8", response.Content.Headers.ContentType.ToString());

            await factory.ExecuteInScopeAsync(async services =>
            {
                var userManager = services.GetService<UserManager>();
                var roleManager = services.GetService<RoleManager>();

                var user = await userManager.FindByNameAsync(username);
                Assert.NotNull(user);

                var isInUserRole = await userManager.IsInRoleAsync(user, Roles.User);
                var isInAdminRole = await userManager.IsInRoleAsync(user, Roles.Admin);
                var isInSuperadminRole = await userManager.IsInRoleAsync(user, Roles.Superadmin);
                Assert.True(isInUserRole);
                Assert.False(isInAdminRole);
                Assert.False(isInSuperadminRole);
                Assert.True(expectedRolesCount == roleManager.Roles.Count());
                Assert.True(expectedUsersCount == userManager.Users.Count());
            });
        }


        private async Task<(User User, IEnumerable<Claim> Claims)> GetSignInUserAsync()
        {
            User user = null;
            IEnumerable<Claim> claims = new List<Claim>();
            await factory.ExecuteInScopeAsync(async services =>
            {
                var userStore = services.GetService<IUserStore<User>>();
                var userManager = services.GetService<UserManager>();
                var signInManager = services.GetService<SignInManager>();

                user = (await userManager.GetUsersInRoleAsync(Roles.User)).First();
                var claimsPrincipal = await signInManager.CreateUserPrincipalAsync(user);
                claims = claimsPrincipal.Claims.ToList();
            });

            return (user, claims);
        }

    }
}
