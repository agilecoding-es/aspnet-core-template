using Template.WebApp.IntegrationTests.Attributes;

namespace Template.WebApp.IntegrationTests.Scenarios.Controllers
{
    [Collection("WebApp")]
    public class ErrorControllerTests
    {
        private readonly WebAppFactory factory;

        public ErrorControllerTests()
        {
            factory = WebAppFactory.GetFactoryInstance();
        }

        [Theory]
        [InlineData("/")]
        [InlineData("/400")]
        [InlineData("/404")]
        [InlineData("/500")]
        [ResetDatabase()]
        public async Task Get_EndpointsReturnSuccessAndCorrectContentType(string url)
        {
            // Arrange
            const string CONTROLLER = "Error";
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
