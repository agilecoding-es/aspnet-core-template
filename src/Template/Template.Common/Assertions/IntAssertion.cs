using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template.Common.Assertions
{
    public class IntAssertion<TException> : Assertion<TException> where TException : Exception
    {
        private readonly int value;

        internal IntAssertion(int value) : base()
        {
            this.value = value;
        }

        public IntAssertion<TException> IsTrue(Func<int, bool> func, string message)
        {
            if (!func.Invoke(value))
                ThrowException(message);

            return this;
        }

        public IntAssertion<TException> IsFalse(Func<int, bool> func, string message)
        {
            if (func.Invoke(value))
                ThrowException(message);

            return this;
        }

        public IntAssertion<TException> IsDefault(string message)
        {
            if (value != default)
                ThrowException(message);

            return this;
        }

        public IntAssertion<TException> IsNotDefault(string message)
        {
            if (value == default)
                ThrowException(message);

            return this;
        }

        public IntAssertion<TException> IsBetween(int minimum, int maximum, string message)
        {
            if (value < minimum || value > maximum)
                ThrowException(message);

            return this;
        }

        public IntAssertion<TException> IsLessThan(int maximun, string message)
        {
            if (value >= maximun)
                ThrowException(message);

            return this;
        }

        public IntAssertion<TException> IsLessThanOrEqual(int maximun, string message)
        {
            if (value > maximun)
                ThrowException(message);

            return this;
        }

        public IntAssertion<TException> IsGreaterThan(int minimum, string message)
        {
            if (value <= minimum)
                ThrowException(message);

            return this;
        }

        public IntAssertion<TException> IsGreaterThanOrEqual(int minimum, string message)
        {
            if (value < minimum)
                ThrowException(message);

            return this;
        }

    }
}
