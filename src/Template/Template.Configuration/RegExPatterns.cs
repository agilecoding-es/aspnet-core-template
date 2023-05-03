using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template.Configuration
{
    public struct RegExPatterns
    {
        public struct Culture
        {
            public const string CULTURE_COOKIE = "(?:c=)(\\w{2}(?:-\\w{2})?)(?:\\|uic=)(\\w{2}(?:-\\w{2})?)";
        }
    }
}
