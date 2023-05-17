﻿using System.Collections;
using System.Linq.Expressions;

namespace Template.Common.Extensions
{
    public static class IEnumerableExtensions
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
