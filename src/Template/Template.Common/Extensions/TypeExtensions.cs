using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Template.Common.Extensions
{
    public static class TypeExtensions
    {
        public static string GetDisplayName<T>(this T someClass)
        {
            var displayName = typeof(T) .GetCustomAttributes(typeof(DisplayNameAttribute), true)
                                                         .FirstOrDefault() as DisplayNameAttribute;

            if (displayName != null)
                return displayName.DisplayName;

            return "";
        }

        public static string GetDisplayNameOrTypeName(this Type entityType)
        {
            var displayName = entityType.GetCustomAttribute<DisplayNameAttribute>();

            if (displayName != null)
                return displayName.DisplayName;

            return entityType.Name;
        }
    }
}
