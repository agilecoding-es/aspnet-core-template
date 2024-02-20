using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template.Common.TypesExtensions
{
    public static class CollectionExtensions
    {
        public static bool IsNullOrEmpty(this IEnumerable enumerable)
        {
            if (enumerable != null)
            {
                foreach (object obj in enumerable)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
