namespace Template.Common.TypesExtensions
{
    public static class BoolExtensions
    {
        public static string AsString(this bool value) => value.ToString().ToLower();
    }
}
