using Template.MvcWebApp.IntegrationTests.Attributes;
using Template.MvcWebApp.IntegrationTests.Fixtures;

namespace Template.MvcWebApp.IntegrationTests.Scenarios
{
    [Collection("WebApp")]
    public class ResetDatabase
    {
        private readonly WebAppFactory factory;
        private readonly UserFixture userFixture;

        public ResetDatabase()
        {
            factory = WebAppFactory.FactoryInstance;
            userFixture = factory.GetService<UserFixture>();
        }

        [Fact]
        [CheckExceptions]
        [ResetDatabase]
        public void Reset() => Assert.True(true);

        [Fact]
        [CheckExceptions]
        [ResetDatabase]
        public async Task ResetAndFillWithUsers()
        {
            await userFixture.SetFixtureAsync();
        }

    }
}
