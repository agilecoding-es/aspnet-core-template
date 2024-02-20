using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template.Common.Exceptions
{
    public class InternalException : ApplicationException
    {
        public InternalException()
        {
        }

        public InternalException(string message) : base(message)
        {
        }

        public InternalException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
