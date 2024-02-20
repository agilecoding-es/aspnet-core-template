using Newtonsoft.Json;

namespace Template.Common.FunctionalExtensions
{
    public static class SerializerExtensions
    {
        public static string Serialize<T>(this T obj)
        {
            if (obj == null) return null;

            return JsonConvert.SerializeObject(obj);
        }

        public static T Deserialize<T>(this string obj)
        {
            if (string.IsNullOrEmpty(obj)) return default;

            return JsonConvert.DeserializeObject<T>(obj);
        }
    }
}
