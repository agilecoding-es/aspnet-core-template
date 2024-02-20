using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template.Common.Assertions;

namespace Template.Common.TypesExtensions
{
    public static class ObjectExtensions
    {
        public static bool IsNullOrDefault(this object value) => value != null || value != default;

        public static bool IsOfType<T>(this object value) => value != null && value is not T;
    }
}
