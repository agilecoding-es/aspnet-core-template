namespace Template.Common
{
    public static class Constants
    {
        public static class Configuration
        {
            public static class ConnectionString
            {
                public const string DefaultConnection = "DefaultConnection";
                public const string AppConnection = "AppConnection";

            }

            public static class Resources
            {
                public const string AppResources = "AppResources";
                public const string DataAnnotation = "DataAnnotationResources";
            }

            public static class Cookies
            {
                public const string AuthCookieName = "TemplateCookie";
                public const string CultureCookieName = "TemplateCultureCookie";
            }
        }

        public static class KeyErrors
        {
            public const string GenericError = "GenericError";
            public const string ValidationError = "ValidationError";
        }
    }
}