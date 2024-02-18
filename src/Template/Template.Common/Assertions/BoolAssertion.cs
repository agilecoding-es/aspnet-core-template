using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template.Common.Assertions
{
    public class BoolAssertion<TException> : Assertion<TException> where TException : Exception
    {
        private readonly bool value;

        internal BoolAssertion(bool value) : base()
        {
            this.value = value;
        }

        public BoolAssertion<TException> IsTrue(string message)
        {
            if (!value)
                ThrowException(message);

            return this;
        }

        public BoolAssertion<TException> IsFalse(string message)
        {
            if (value)
                ThrowException(message);

            return this;
        }

    }
}
