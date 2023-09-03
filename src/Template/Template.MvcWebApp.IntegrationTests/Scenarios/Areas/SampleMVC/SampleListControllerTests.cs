using AngleSharp.Html.Dom;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Security.Claims;
using Template.Application.Identity;
using Template.Domain.Entities.Identity;
using Template.MvcWebApp.IntegrationTests.Attributes;
using Template.MvcWebApp.IntegrationTests.Extensions;
using Template.MvcWebApp.IntegrationTests.Fixtures;
using Template.Security.Authorization;

namespace Template.MvcWebApp.IntegrationTests.Scenarios.Areas.SampleMVC
{
    [Collection("WebApp")]
    public class SampleListControllerTests
    {
        const string AREA = "SampleMVC";
        const string CONTROLLER = "SampleList";

        private readonly WebAppFactory factory;
        private readonly UserFixture userFixture;
        private readonly SampleListFixture sampleListFixture;

        public SampleListControllerTests()
        {
            factory = WebAppFactory.GetFactoryInstance();
            userFixture = factory.GetService<UserFixture>();
            sampleListFixture = factory.GetService<SampleListFixture>();
        }

        #region Check Endpoinsts response content-type

        [Theory]
        [InlineData("/Index")]
        [InlineData("/Detail")]
        [InlineData("/Create")]
        [InlineData("/Edit")]
        [InlineData("/Delete")]
        [CheckExceptions()]
        [ResetDatabase()]
        public async Task GetEndpoints_WhenUserIsNotAuthenticated_RedirectsToLoginPage(string endpoint)
        {
            // Arrange
            var url = $"{AREA}/{CONTROLLER}{endpoint}";

            // Act
            var response = await factory
                .CreateClient(new WebApplicationFactoryClientOptions
                {
                    AllowAutoRedirect = false
                })
                .GetAsync(url);

            // Assert
            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
            Assert.StartsWith("http://localhost/Identity/Account/Login",
                               response.Headers.Location.OriginalString);
        }

        [Theory]
        [InlineData("/Index")]
        [InlineData("/Create")]
        [InlineData("/Delete")]
        [CheckExceptions()]
        [ResetDatabase()]
        public async Task GetEndpoints_WhenUserIsAuthenticated_ReturnSuccessAndCorrectContentType(string endpoint)
        {
            // Arrange
            var url = $"{AREA}/{CONTROLLER}{endpoint}";

            await userFixture.SetFixtureAsync();
            var (user, claims) = await GetSignInUserAsync();

            // Act
            var response = await factory
                .CreateClient(new WebApplicationFactoryClientOptions
                {
                    AllowAutoRedirect = false
                }).WithDefaultIdentity(claims)
                .GetAsync(url);

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            Assert.Equal("text/html; charset=utf-8",
                response.Content.Headers.ContentType.ToString());
        }

        [Theory]
        [InlineData("/Detail")]
        [InlineData("/Edit")]
        [CheckExceptions()]
        [ResetDatabase()]
        public async Task GetEndpoints_WhenUserIsAuthenticatedAndListExists_ReturnSuccessAndCorrectContentType(string endpoint)
        {
            // Arrange
            await userFixture.SetFixtureAsync();
            var (user, claims) = await GetSignInUserAsync();

            var lists = await sampleListFixture.SetFixtureAsync(user);

            var url = $"{AREA}/{CONTROLLER}{endpoint}/{lists.First().Id}";

            // Act
            var response = await factory.CreateRequest(url)
                .WithIdentity(claims)
                .GetAsync();

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            Assert.Equal("text/html; charset=utf-8",
                response.Content.Headers.ContentType.ToString());
        }

        [Theory]
        [InlineData("/Detail")]
        [InlineData("/Edit")]
        [CheckExceptions()]
        [ResetDatabase()]
        public async Task GetEndpoints_WhenUserIsAuthenticatedAndListNotExists_ReturnSuccessAndCorrectContentType(string endpoint)
        {
            // Arrange
            await userFixture.SetFixtureAsync();
            var (user, claims) = await GetSignInUserAsync();

            var url = $"{AREA}/{CONTROLLER}{endpoint}/0";

            // Act
            var response = await factory
                .CreateClient(
                new WebApplicationFactoryClientOptions()
                {
                    AllowAutoRedirect = false
                })
                .WithDefaultIdentity(claims)
                .GetAsync(url);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        #endregion

        #region CRUD operations

        [Fact]
        [CheckExceptions()]
        [ResetDatabase()]
        public async Task Index_WhenNoListExists_ShowCreateNewButtonInCard()
        {
            // Arrange
            var url = $"{AREA}/{CONTROLLER}/Index";

            await userFixture.SetFixtureAsync();
            var (user, claims) = await GetSignInUserAsync();

            // Act
            var response = await factory
                .CreateClient()
                .WithDefaultIdentity(claims)
                .GetAsync(url);

            // Assert

            response.EnsureSuccessStatusCode();
            var content = await response.GetDocumentAsync();
            var createButton = (IHtmlAnchorElement)content.QuerySelector(".grid .js_CreateButton");

            Assert.NotNull(createButton);
        }

        #endregion

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
