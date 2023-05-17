using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template.Configuration
{
    public struct Constants
    {
        public struct Configuration
        {

            public struct ConnectionString
            {
                public const string DEFAULT_CONNECTION = "DefaultConnection";

            }
            public struct Resources
            {
                public const string DEFAULT = "AppResources";
                public const string DATANNOTATION = "DataAnnotationResources";

            }

            public struct Cookies
            {
                public const string CULTURE_COOKIE = "TemplateCultureCookie";
            }
        }

    }
}
