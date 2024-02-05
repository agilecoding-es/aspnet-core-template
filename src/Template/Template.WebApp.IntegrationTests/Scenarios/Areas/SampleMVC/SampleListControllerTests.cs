using AngleSharp.Html.Dom;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Security.Claims;
using System.Text.RegularExpressions;
using Template.Application.Features.SampleContext.Contracts;
using Template.Domain.Entities.Identity;
using Template.WebApp.IntegrationTests.Attributes;
using Template.WebApp.IntegrationTests.Extensions;
using Template.WebApp.IntegrationTests.Fixtures;
using Template.Security.Authorization;
using Template.Application.Features.IdentityContext.Services;

namespace Template.WebApp.IntegrationTests.Scenarios.Areas.SampleMVC
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
            userFixture = new UserFixture(factory);
            sampleListFixture = new SampleListFixture(factory);
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
        public async Task GetEndpoints_WhenUserIsNotAuthenticated_ReturnsUnauthorized(string endpoint)
        {
            // Arrange
            var url = $"{AREA}/{CONTROLLER}{endpoint}";

            // Act
            var response = await factory.CreateClient().GetAsync(url);

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
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

        [Fact]
        [CheckExceptions()]
        [ResetDatabase()]
        public async Task Create_WithValidParameters_AddsNewList()
        {
            // Arrange
            await userFixture.SetFixtureAsync();
            var (user, claims) = await GetSignInUserAsync();

            var lists = await sampleListFixture.SetFixtureAsync(user);
            var expectedListCount = lists.Count + 1;

            var url = $"{AREA}/{CONTROLLER}/Create";

            var model = new Dictionary<string, string>()
            {
                { "Name" , "Test List"},
            };

            var pattern = @"\d+$";

            // Act 
            var response =
                await factory.CreateClient()
                       .WithDefaultIdentity(claims)
                       .PostAsync(url, model.AsFormUrlEncodedContent());

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Contains($"/{AREA}/{CONTROLLER}/Edit/".ToLower(), response.RequestMessage.RequestUri.ToString().ToLower());
            var id = new Regex(pattern).Match(response.RequestMessage.RequestUri.ToString())?.Value;

            await factory.ExecuteInScopeAsync(async services =>
            {
                var sampleListRepository = services.GetService<ISampleListRepository>();
                var sampleLists = await sampleListRepository.ListAsync(l => l.UserId == user.Id, CancellationToken.None);
                var lastCreated = sampleLists.OrderByDescending(s => s.Id).First();

                Assert.Equal(lastCreated.Id, int.Parse(id));
                Assert.True(expectedListCount == sampleLists.Count());
            });
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
