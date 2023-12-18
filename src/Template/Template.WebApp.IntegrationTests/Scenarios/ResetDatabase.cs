using Template.Common;
using Template.WebApp.IntegrationTests.Attributes;
using Template.WebApp.IntegrationTests.Fixtures;

namespace Template.WebApp.IntegrationTests.Scenarios
{
    [Collection("WebApp")]
    [Trait("Category", "ExcludedFromBuild")]
    public class ResetDatabase
    {
        private readonly WebAppFactory factory;
        private readonly UserFixture userFixture;

        public ResetDatabase()
        {
            factory = WebAppFactory.GetFactoryInstance(Constants.Configuration.ConnectionString.AppConnection);
            userFixture = new UserFixture(factory);
        }

        [Fact]
        [CheckExceptions]
        [ResetDatabase]
        public void Reset() => Assert.True(true);

        [Fact]
        [CheckExceptions]
        [ResetDatabase]
        public async Task ResetAndFillWithUsers() => await userFixture.SetFixtureAsync();

    }
}
