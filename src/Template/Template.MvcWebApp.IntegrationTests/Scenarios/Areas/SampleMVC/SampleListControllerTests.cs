using Template.MvcWebApp.IntegrationTests.Attributes;

namespace Template.MvcWebApp.IntegrationTests.Scenarios.Areas.SampleMVC
{
    [Collection("WebApp")]
    public class SampleListControllerTests 
    {
        private readonly WebAppFactory factory;

        public SampleListControllerTests()
        {
            factory = WebAppFactory.FactoryInstance;
        }


        [Theory]
        [InlineData("/Index")]
        [InlineData("/Detail")]
        [InlineData("/Create")]
        [InlineData("/Edit")]
        [InlineData("/Delete")]
        [ResetDatabase()]
        public async Task Get_EndpointsReturnSuccessAndCorrectContentType(string url)
        {
            // Arrange
            const string AREA = "SampleMVC";
            const string CONTROLLER = "SampleList";
            
            // Act
            var response = await factory.CreateClient().GetAsync($"{AREA}/{CONTROLLER}{url}");

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            Assert.Equal("text/html; charset=utf-8",
                response.Content.Headers.ContentType.ToString());
        }
    }
}
