using System.ComponentModel;
using System.Reflection;

namespace Template.Common.FunctionalExtensions
{
    public static class EnumExtensions
    {
        /// <summary>
        /// The GetDescription.
        /// </summary>
        /// <param name="value">The value<see cref="Enum"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public static string GetDescription(this Enum value)
        {
            Type type = value.GetType();
            string name = Enum.GetName(type, value);

            if (name != null)
            {
                FieldInfo field = type.GetField(name);
                if (field != null)
                {
                    DescriptionAttribute attr = field.GetCustomAttribute<DescriptionAttribute>();
                    if (attr != null)
                    {
                        return attr.Description;
                    }
                }
            }

            return value.ToString();
        }

        /// <summary>
        /// The GetDescriptions.
        /// </summary>
        /// <param name="value">The value<see cref="Type"/>.</param>
        /// <returns>The <see cref="List{string}"/>.</returns>
        public static IEnumerable<string> GetDescriptions(this Enum value)
        {
            Type type = value.GetType();
            Array enumValues = Enum.GetValues(type);

            foreach (Enum e in enumValues)
            {
                yield return GetDescription(e);
            }
        }

        /// <summary>
        /// The FromDescription.
        /// </summary>
        /// <param name="type">The type<see cref="Type"/>.</param>
        /// <param name="description">The attribute<see cref="string"/>.</param>
        /// <returns>The <see cref="Enum"/>.</returns>
        public static Enum FromDescription(this Enum value, string description)
        {
            Type type = value.GetType();

            Array values = Enum.GetValues(type);

            foreach (Enum e in values)
            {
                if (e.GetDescription().Equals(description))
                    return e;
            }
            return null;
        }
    }
}
