using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Globalization;

namespace Template.MvcWebApp.IntegrationTests.Extensions
{
    internal static class TestObjectExtensions
    {
        public static IDictionary<string, string> ToKeyValue(this object metaToken)
        {
            if (metaToken == null)
            {
                return null;
            }

            JToken token = metaToken as JToken;
            if (token == null)
            {
                return ToKeyValue(JObject.FromObject(metaToken));
            }

            if (token.HasValues)
            {
                var contentData = new Dictionary<string, string>();
                foreach (var child in token.Children().ToList())
                {
                    var childContent = child.ToKeyValue();
                    if (childContent != null)
                    {
                        contentData = contentData.Concat(childContent)
                                                 .ToDictionary(k => k.Key, v => v.Value);
                    }
                }

                return contentData;
            }

            var jValue = token as JValue;
            if (jValue?.Value == null)
            {
                return null;
            }

            var value = jValue?.Type == JTokenType.Date ?
                            jValue?.ToString("o", CultureInfo.InvariantCulture) :
                            jValue?.ToString(CultureInfo.InvariantCulture);

            return new Dictionary<string, string> { { token.Path, value } };
        }

        public static string AsQueryString(this object model) => model.AsQueryStringAsync().GetAwaiter().GetResult();

        public static async Task<string> AsQueryStringAsync(this object model) => await model.AsFormUrlEncodedContent().ReadAsStringAsync();

        public static StringContent AsJsonStringContent(this object model) => JsonConvert.SerializeObject(model).AsStringContent();

        public static StringContent AsFormUrlEncodedStringContent(this object model) => model.AsQueryString().AsFormUrlEncodedStringContent();

        public static FormUrlEncodedContent AsFormUrlEncodedContent(this object model)
        {
            var keyValueContent = model.ToKeyValue();
            var formUrlEncodedContent = new FormUrlEncodedContent(keyValueContent);
            return formUrlEncodedContent;
        }
    }
}
