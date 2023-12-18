using System.Reflection;
using Xunit.Sdk;

namespace Template.WebApp.IntegrationTests.Attributes
{
    internal class CheckExceptionsAttribute : BeforeAfterTestAttribute
    {
        private int _exceptionsBefore = 0;
        public override void Before(MethodInfo methodUnderTest)
        {
            _exceptionsBefore = WebAppFactory.GetExceptionsCount();
        }

        public override void After(MethodInfo methodUnderTest)
        {
            var exceptionsAfter = WebAppFactory.GetExceptionsCount();

            Assert.Equal(_exceptionsBefore, exceptionsAfter);
        }
    }
}
