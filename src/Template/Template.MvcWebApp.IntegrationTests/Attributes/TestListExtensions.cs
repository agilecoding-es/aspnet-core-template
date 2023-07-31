using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template.MvcWebApp.IntegrationTests.Extensions
{
    internal static class TestListExtensions
    {
        public static string ToParametrizedString<T>(this List<T> list, string paramName) =>
         (list?.Count > 0 ? string.Join("", list.Select(s => $"&{paramName}={s}")) : "");

    }
}
