using AngleSharp.Html.Dom;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Org.BouncyCastle.Ocsp;
using System.IO;
using System.Net;
using System.Security.Claims;
using Template.Application.Identity;
using Template.MvcWebApp.IntegrationTests.Attributes;
using Template.MvcWebApp.IntegrationTests.Extensions;
using Template.MvcWebApp.IntegrationTests.Fixtures;
using Template.Security.Authorization;

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
            userFixture = factory.GetService<UserFixture>();
        }

        [Fact(Skip ="Temporalmente ")]
        [CheckExceptions()]
        [ResetDatabase()]
        public async Task WhenUserPostRegister_WithValidParameters_UserIsCreatedAndAssignToRoleUser()
        {
            // Arrange
            await userFixture.SetFixtureAsync();

            var url = $"{AREA}/{PATH}/{PAGE}";
            var username = "TestUser";

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

            // Realiza una solicitud GET para obtener el formulario de registro
            var getResponse = await client.GetAsync(url);

            getResponse.EnsureSuccessStatusCode();
            var content = await getResponse.GetDocumentAsync();
            var verificationToken = (IHtmlInputElement)content.QuerySelector("input[name='__RequestVerificationToken']");

            var model = new Dictionary<string, string>()
            {
                { "Input.UserName" , username},
                { "Input.Email" , "testuser@sample.com"},
                { "Input.Password" , "123456"},
                { "Input.ConfirmPassword" , "123456" },
                { "__RequestVerificationToken" , verificationToken.GetAttribute("Value") },
            };
            
            // Act
            var response =
                await factory.CreateRequest(url)
                       .And(req => req.Content = model.AsFormUrlEncodedStringContent()).PostAsync();

            //// Act
            //var response =
            //    await factory.CreateRequest(url)
            //           .And(req => req.Content = model.AsFormUrlEncodedContent()).PostAsync();

            //// Act
            //var response = await client.PostAsync(url, model.AsFormUrlEncodedContent());

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
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
    }
}
