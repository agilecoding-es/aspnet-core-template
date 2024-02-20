using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template.Common.Assertions
{
    public class ShortAssertion<TException> : Assertion<TException> where TException : Exception
    {
        private readonly short value;

        internal ShortAssertion(short value) : base()
        {
            this.value = value;
        }

        public ShortAssertion<TException> IsTrue(Func<int, bool> func, string message)
        {
            if (!func.Invoke(value))
                ThrowException(message);

            return this;
        }

        public ShortAssertion<TException> IsFalse(Func<int, bool> func, string message)
        {
            if (func.Invoke(value))
                ThrowException(message);

            return this;
        }

        public ShortAssertion<TException> IsDefault(string message)
        {
            if (value != default)
                ThrowException(message);

            return this;
        }

        public ShortAssertion<TException> IsNotDefault(string message)
        {
            if (value == default)
                ThrowException(message);

            return this;
        }

        public ShortAssertion<TException> IsBetween(short minimum, short maximum, string message)
        {
            if (value < minimum || value > maximum)
                ThrowException(message);

            return this;
        }

        public ShortAssertion<TException> IsLessThan(short maximun, string message)
        {
            if (value >= maximun)
                ThrowException(message);

            return this;
        }

        public ShortAssertion<TException> IsLessThanOrEqual(short maximun, string message)
        {
            if (value > maximun)
                ThrowException(message);

            return this;
        }

        public ShortAssertion<TException> IsGreaterThan(short minimum, string message)
        {
            if (value <= minimum)
                ThrowException(message);

            return this;
        }

        public ShortAssertion<TException> IsGreaterThanOrEqual(short minimum, string message)
        {
            if (value < minimum)
                ThrowException(message);

            return this;
        }

    }
}
