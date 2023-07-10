using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template.Domain.Entities.Shared
{
    public static class Errors
    {
        public static readonly Error None = new(string.Empty, string.Empty);
        public static readonly Error NullValue = new("Error.NullValue", "The specified result value is null");

    }
}
