using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template.Common.Extensions
{
    public static class BoolExtensions
    {
        public static string AsString(this bool value) => value.ToString().ToLower();
    }
}
