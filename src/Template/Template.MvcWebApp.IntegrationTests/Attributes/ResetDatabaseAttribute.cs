using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Template.MvcWebApp.IntegrationTests.Extensions;
using Xunit.Sdk;

namespace Template.MvcWebApp.IntegrationTests.Attributes
{
    internal class ResetDatabaseAttribute : BeforeAfterTestAttribute
    {
        private readonly IServiceCollection services;
        private readonly IConfiguration configuration;

        public ResetDatabaseAttribute(WebAppFactory factory)
        {
            this.services = services;
            this.configuration = configuration;
        }

        public override void Before(MethodInfo methodUnderTest) => services.ResetDatabase(configuration).GetAwaiter().GetResult();
    }
}
