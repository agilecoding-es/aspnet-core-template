using System.Reflection;
using Template.Common;
using Xunit.Sdk;

namespace Template.WebApp.IntegrationTests.Attributes
{
    internal class ResetDatabaseAttribute : BeforeAfterTestAttribute
    {
        private readonly string connectionStringName;

        public ResetDatabaseAttribute(string connectionStringName = Constants.Configuration.ConnectionString.DefaultConnection)
        {
            this.connectionStringName = connectionStringName;
        }

        public override void Before(MethodInfo methodUnderTest) => WebAppFactory.ResetDatabase();
    }
}
