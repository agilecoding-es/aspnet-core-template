using Template.MvcWebApp.IntegrationTests.Attributes;

namespace Template.MvcWebApp.IntegrationTests.Scenarios.Controllers
{
    [Collection("WebApp")]
    public class HomeControllerTests 
    {
        private readonly HttpClient client;

        public HomeControllerTests()
        {
            var factory = WebAppFactory.FactoryInstance;
            client = factory.SharedHttpClient;
        }

        [Theory]
        [InlineData("/")]
        [InlineData("/Index")]
        [InlineData("/Privacy")]
        [InlineData("/Error")]
        [ResetDatabase()]
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
    }
}
