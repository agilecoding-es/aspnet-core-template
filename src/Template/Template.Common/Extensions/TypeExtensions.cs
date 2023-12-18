using System.ComponentModel;
using System.Reflection;

namespace Template.Common.Extensions
{
    public static class TypeExtensions
    {
        public static string GetDisplayName<T>(this T someClass)
        {
            var displayName = typeof(T).GetCustomAttributes(typeof(DisplayNameAttribute), true)
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
