using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Security.Policy;
using Template.MvcWebApp.IntegrationTests.Attributes;
using Xunit;

namespace Template.MvcWebApp.IntegrationTests.Scenarios.Areas.SampleMVC
{
    [Collection("WebApp")]
    public class SampleListControllerTests : IAsyncLifetime
    {
        private readonly HttpClient client;
        private readonly Func<Task> resetDatabase;

        public SampleListControllerTests(WebAppFactory factory)
        {
            client = factory.HttpClient;
            resetDatabase = factory.ResetDatabase;
        }


        [Theory]
        [InlineData("/Index")]
        [InlineData("/Detail")]
        [InlineData("/Create")]
        [InlineData("/Edit")]
        [InlineData("/Delete")]
        //[ResetDatabase()]
        public async Task Get_EndpointsReturnSuccessAndCorrectContentType(string url)
        {
            // Arrange
            const string AREA = "SampleMVC";
            const string CONTROLLER = "SampleList";
            
            // Act
            var response = await client.GetAsync($"{AREA}/{CONTROLLER}{url}");

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            Assert.Equal("text/html; charset=utf-8",
                response.Content.Headers.ContentType.ToString());
        }

        public Task InitializeAsync() => Task.CompletedTask;
        public Task DisposeAsync() => resetDatabase();
    }
}
