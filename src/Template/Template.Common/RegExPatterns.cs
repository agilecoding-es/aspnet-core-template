namespace Template.Common
{
    public static class RegExPatterns
    {
        public static class Culture
        {
            public const string CultureCookie = "(?:c=)(\\w{2}(?:-\\w{2})?)(?:\\|uic=)(\\w{2}(?:-\\w{2})?)";
        }

        public static class Validators
        {
            public const string Username = @"^(?!\s|\p{P}|\p{S}|\p{Z})\w+$";
            public const string Email = @"^\w+([-+._]\w+)*(?:@\w+([-]\w+)*\.[a-zA-Z]{2,}([.]\w{2})?)$";
            public const string UsernameOrEmail = @"^(?:(?!\s|\p{P}|\p{S}|\p{Z})\w+|\w+([-+._]\w+)*(?:@\w+([-]\w+)*\.[a-zA-Z]{2,}([.]\w{2})?))$";
            public const string Guid = @"^[a-fA-F0-9]{8}[-](?:[a-fA-F0-9]{4}[-]){3}[a-fA-F0-9]{12}$";
        }
    }
}
