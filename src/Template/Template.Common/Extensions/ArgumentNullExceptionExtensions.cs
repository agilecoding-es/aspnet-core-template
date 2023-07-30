using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template.Common.Extensions
{
    public static class ArgumentNullExceptionExtensions
    {
       public static void ThrowIfNull(this ArgumentNullException argumentNullException, object param)
        {
            if(param == null)
                throw new ArgumentNullException(nameof(param));
        }
    }
}
