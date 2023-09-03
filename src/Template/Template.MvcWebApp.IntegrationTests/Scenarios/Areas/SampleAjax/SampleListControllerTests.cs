using AutoMapper.Configuration.Annotations;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using System.Net;
using System.Security.Claims;
using Template.MvcWebApp.IntegrationTests.Attributes;

namespace Template.MvcWebApp.IntegrationTests.Scenarios.Areas.SampleAjax
{
    [Collection("WebApp")]
    public class SampleListControllerTests
    {
        const string AREA = "SampleAjax";
        const string CONTROLLER = "SampleList";
        private readonly WebAppFactory factory;

        public SampleListControllerTests()
        {
            factory = WebAppFactory.FactoryInstance;
        }

        //[Theory(Skip = "Pendiente de revision")]
        [Theory()]
        [InlineData("/Index")]
        [InlineData("/Detail")]
        [InlineData("/Create")]
        [InlineData("/Edit")]
        [InlineData("/Delete")]
        [CheckExceptions()]
        public async Task Get_SecurePageRequiresAnAuthenticatedUser(string endpoint)
        {
            // Arrange
            var url = $"{AREA}/{CONTROLLER}{endpoint}";
            var client = factory.CreateClient(
                new WebApplicationFactoryClientOptions
                {
                    AllowAutoRedirect = false
                });

            // Act
            var response = await client.GetAsync(url);

            // Assert
            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
            Assert.StartsWith("http://localhost/Identity/Account/Login",
                               response.Headers.Location.OriginalString);
        }

        [Theory(Skip = "Pendiente de revision")]
        //[Theory()]
        [InlineData("/Index")]
        [InlineData("/Detail")]
        [InlineData("/Create")]
        [InlineData("/Edit")]
        [InlineData("/Delete")]
        [ResetDatabase()]
        public async Task Get_EndpointsReturnSuccessAndCorrectContentType(string endpoint)
        {
            // Arrange
            var url = $"{AREA}/{CONTROLLER}{endpoint}";
            var request =
                factory.CreateRequest(url)
                       .WithIdentity(new List<Claim>
                       {
                           new Claim(ClaimTypes.NameIdentifier, ""), //UserSettings.UserId)
                        new Claim(ClaimTypes.Name, ""), //UserSettings.Name),
                        new Claim(ClaimTypes.Email, ""), //UserSettings.UserEmail),
                        new Claim(ClaimTypes.Role, "Admin")
                       });

            // Act
            var response = await request.GetAsync();

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("text/html; charset=utf-8",
                response.Content.Headers.ContentType.ToString());
        }

    }
}
