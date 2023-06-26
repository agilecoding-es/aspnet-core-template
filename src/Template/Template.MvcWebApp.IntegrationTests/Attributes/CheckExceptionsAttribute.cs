using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xunit.Sdk;

namespace Template.MvcWebApp.IntegrationTests.Attributes
{
    internal class CheckExceptionsAttribute : BeforeAfterTestAttribute
    {
        //private int _exceptionsBefore = 0;

        //public override void Before(MethodInfo methodUnderTest) => _exceptionsBefore = TestFixture.GetExceptionsCount().GetAwaiter().GetResult();

        //public override void After(MethodInfo methodUnderTest)
        //{
        //    var exceptionsAfter = TestFixture.GetExceptionsCount().GetAwaiter().GetResult();

        //    Assert.Equal(_exceptionsBefore, exceptionsAfter);
        //}
    }
}
