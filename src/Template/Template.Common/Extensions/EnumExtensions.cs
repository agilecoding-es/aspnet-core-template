using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Template.Common.Extensions
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
            // Get the type
            Type type = value.GetType();

            // Get fieldinfo for this type
            FieldInfo fieldInfo = type.GetField(value.ToString());

            // Get the stringvalue attributes
            DescriptionAttribute[] attribs = fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[];

            // Return the first if there was a match.
            return attribs.Length > 0 ? attribs[0].Description : null;
        }

        /// <summary>
        /// The GetDescriptions.
        /// </summary>
        /// <param name="value">The value<see cref="Type"/>.</param>
        /// <returns>The <see cref="List{string}"/>.</returns>
        public static List<string> GetDescriptions(this Type value)
        {
            List<string> values = new List<string>();
            Array enumValues = Enum.GetValues(value);

            foreach (Enum e in enumValues)
            {
                values.Add(e.GetDescription());
            }

            return values;
        }

        /// <summary>
        /// The GetEnumFromAttribute.
        /// </summary>
        /// <param name="enType">The enType<see cref="Type"/>.</param>
        /// <param name="attribute">The attribute<see cref="string"/>.</param>
        /// <returns>The <see cref="Enum"/>.</returns>
        public static Enum GetEnumFromAttribute(Type enType, string attribute)
        {
            Enum eRet = null;

            Array vals = Enum.GetValues(enType);
            foreach (Enum e in vals)
            {
                string att = e.GetDescription();
                if (att.Equals(attribute))
                {
                    eRet = e;
                    break;
                }
            }
            return eRet;
        }
    }
}
