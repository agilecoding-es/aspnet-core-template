using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template.Common.Assertions
{
    public class LongAssertion<TException> : Assertion<TException> where TException : Exception
    {
        private readonly long value;

        internal LongAssertion(long value) : base()
        {
            this.value = value;
        }

        public LongAssertion<TException> IsTrue(Func<long, bool> func, string message)
        {
            if (!func.Invoke(value))
                ThrowException(message);

            return this;
        }

        public LongAssertion<TException> IsFalse(Func<long, bool> func, string message)
        {
            if (func.Invoke(value))
                ThrowException(message);

            return this;
        }

        public LongAssertion<TException> IsDefault(string message)
        {
            if (value != default)
                ThrowException(message);

            return this;
        }

        public LongAssertion<TException> IsNotDefault(string message)
        {
            if (value == default)
                ThrowException(message);

            return this;
        }

        public LongAssertion<TException> IsBetween(long minimum, long maximum, string message)
        {
            if (value < minimum || value > maximum)
                ThrowException(message);

            return this;
        }

        public LongAssertion<TException> IsLessThan(long maximun, string message)
        {
            if (value >= maximun)
                ThrowException(message);

            return this;
        }

        public LongAssertion<TException> IsLessThanOrEqual(long maximun, string message)
        {
            if (value > maximun)
                ThrowException(message);

            return this;
        }

        public LongAssertion<TException> IsGreaterThan(long minimum, string message)
        {
            if (value <= minimum)
                ThrowException(message);

            return this;
        }

        public LongAssertion<TException> IsGreaterThanOrEqual(long minimum, string message)
        {
            if (value < minimum)
                ThrowException(message);

            return this;
        }

    }
}
