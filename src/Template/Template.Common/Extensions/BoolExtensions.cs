namespace Template.Common.Extensions
{
    public static class BoolExtensions
    {
        public static string AsString(this bool value) => value.ToString().ToLower();
    }
}
