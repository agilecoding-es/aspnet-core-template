namespace Template.MvcWebApp.IntegrationTests.Scenarios.Areas.SampleAjax
{
    [Collection("WebApp")]
    public class SampleItemControllerTests
    {
        private readonly HttpClient client;

        public SampleItemControllerTests()
        {
            var factory = WebAppFactory.FactoryInstance;
            client = factory.SharedHttpClient;
        }

    }
}
