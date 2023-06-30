using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Template.MvcWebApp.IntegrationTests.Extensions;
using Xunit.Sdk;

namespace Template.MvcWebApp.IntegrationTests.Attributes
{
    internal class ResetDatabaseAttribute : BeforeAfterTestAttribute
    {
        public override void Before(MethodInfo methodUnderTest) => WebAppFactory.ResetDatabaseAsync().GetAwaiter().GetResult();
    }
}
