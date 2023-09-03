using Template.MvcWebApp.IntegrationTests.Attributes;

namespace Template.MvcWebApp.IntegrationTests.Scenarios.Controllers
{
    [Collection("WebApp")]
    public class HomeControllerTests 
    {
        private readonly WebAppFactory factory;

        public HomeControllerTests()
        {
            factory = WebAppFactory.GetFactoryInstance();
        }

        [Theory]
        [InlineData("/")]
        [InlineData("/Index")]
        [InlineData("/Privacy")]
        [ResetDatabase()]
        public async Task Get_EndpointsReturnSuccessAndCorrectContentType(string url)
        {
            // Arrange
            const string CONTROLLER = "Home";
            var client = factory.CreateClient();

            // Act
            var response = await client.GetAsync($"{CONTROLLER}{url}");

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            Assert.Equal("text/html; charset=utf-8",
                response.Content.Headers.ContentType.ToString());
        }
    }
}
