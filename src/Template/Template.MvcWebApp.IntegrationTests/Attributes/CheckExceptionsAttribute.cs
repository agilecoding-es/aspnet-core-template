using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Template.MvcWebApp.IntegrationTests.Queries;
using Xunit.Sdk;

namespace Template.MvcWebApp.IntegrationTests.Attributes
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
