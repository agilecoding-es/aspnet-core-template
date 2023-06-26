using Microsoft.AspNetCore.Mvc.Testing;

namespace Template.MvcWebApp.IntegrationTests.Scenarios.Controllers
{
    [Collection("WebApp")]
    public class HomeControllerTests : IAsyncLifetime
    {
        private readonly HttpClient client;
        private readonly Func<Task> resetDatabase;

        public HomeControllerTests(WebAppFactory factory)
        {
            client = factory.HttpClient;
            resetDatabase = factory.ResetDatabase;
        }

        [Theory]
        [InlineData("/")]
        [InlineData("/Index")]
        [InlineData("/Privacy")]
        [InlineData("/Error")]
        public async Task Get_EndpointsReturnSuccessAndCorrectContentType(string url)
        {
            // Arrange
            const string CONTROLLER = "Home";

            // Act
            var response = await client.GetAsync($"{CONTROLLER}{url}");

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            Assert.Equal("text/html; charset=utf-8",
                response.Content.Headers.ContentType.ToString());
        }

        public Task InitializeAsync() => Task.CompletedTask;
        public Task DisposeAsync() => resetDatabase();
    }
}
