namespace Template.Common
{
    public struct Constants
    {
        public struct Configuration
        {
            public struct ConnectionString
            {
                public static ConnectionString DefaultConnection = new ConnectionString("DefaultConnection");

                public ConnectionString(string value)
                {
                    Value = value;
                }

                public string Value { get; }
            }

            public struct Resources
            {
                public static Resources AppResources = new Resources("AppResources");
                public static Resources DataAnnotation = new Resources("DataAnnotationResources");

                public Resources(string value)
                {
                    Value = value;
                }

                public string Value { get; }
            }

            public struct Cookies
            {
                public static Cookies CultureCookieName = new Cookies("TemplateCultureCookie");

                public Cookies(string value)
                {
                    Value = value;
                }

                public string Value { get; }
            }
        }

        public struct KeyErrors
        {
            public static KeyErrors ValidationError = new KeyErrors("ValidationError");

            public KeyErrors(string value)
            {
                Value = value;
            }

            public string Value { get; }
        }

    }
}