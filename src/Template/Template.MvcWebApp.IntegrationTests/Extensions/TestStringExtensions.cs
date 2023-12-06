using System.Text;

namespace Template.MvcWebApp.IntegrationTests.Extensions
{
    internal static class TestStringExtensions
    {
        public static StringContent AsFormUrlEncodedStringContent(this string content) =>
                new StringContent(content, Encoding.UTF8, "application/X-www-form-urlencoded");

        public static StringContent AsStringContent(this string content) =>
            new StringContent(content, Encoding.UTF8, "application/json");

    }
}
