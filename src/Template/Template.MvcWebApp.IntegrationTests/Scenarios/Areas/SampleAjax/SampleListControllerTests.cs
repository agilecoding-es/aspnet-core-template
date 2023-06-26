using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template.MvcWebApp.IntegrationTests.Attributes;

namespace Template.MvcWebApp.IntegrationTests.Scenarios.Areas.SampleAjax
{
    [Collection("WebApp")]
    public class SampleListControllerTests
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
        public async Task Get_EndpointsReturnSuccessAndCorrectContentType(string url)
        {
            // Arrange
            const string AREA = "SampleAjax";
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
