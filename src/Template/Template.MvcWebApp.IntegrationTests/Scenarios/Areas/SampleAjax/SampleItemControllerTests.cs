using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template.MvcWebApp.IntegrationTests.Scenarios.Areas.SampleAjax
{
    [Collection("WebApp")]
    public class SampleItemControllerTests
    {
        private readonly HttpClient client;
        private readonly Func<Task> resetDatabase;

        public SampleItemControllerTests(WebAppFactory factory)
        {
            client = factory.HttpClient;
            resetDatabase = factory.ResetDatabase;
        }

        public Task InitializeAsync() => Task.CompletedTask;
        public Task DisposeAsync() => resetDatabase();
    }
}
