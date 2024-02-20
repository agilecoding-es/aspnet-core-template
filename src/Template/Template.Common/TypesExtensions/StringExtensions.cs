using System.Text.RegularExpressions;

namespace Template.Common.TypesExtensions
{
    public static class StringExtensions
    {
        public static bool IsNullOrEmpty(this string value) => string.IsNullOrEmpty(value);
        public static bool IsNullOrWhiteSpace(this string value) => string.IsNullOrWhiteSpace(value);
        public static string PascalCaseToSnakeCase(this string value)
        {
            if (string.IsNullOrEmpty(value)) return value;

            var splitted = new Regex("(?=[A-Z])").Split(value);
            return string.Join('_', splitted, 1, splitted.Length - 1).ToLower();
        }
    }
}
