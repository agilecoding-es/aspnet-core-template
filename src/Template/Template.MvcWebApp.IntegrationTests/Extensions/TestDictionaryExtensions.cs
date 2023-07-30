using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template.MvcWebApp.IntegrationTests.Extensions
{
    internal static class TestDictionaryExtensions
    {
        public static StringContent AsFormUrlEncodedStringContent(this Dictionary<string, string> model) => model.AsFormUrlEncodedStringContentAsync().GetAwaiter().GetResult();

        public static async Task<StringContent> AsFormUrlEncodedStringContentAsync(this Dictionary<string, string> model) => (await model.AsFormUrlEncodedContent().ReadAsStringAsync()).AsFormUrlEncodedStringContent();

        public static FormUrlEncodedContent AsFormUrlEncodedContent(this Dictionary<string, string> model) => new FormUrlEncodedContent(model);
    }
}
