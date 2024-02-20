using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template.Common.Assertions
{
    public class DateTimeAssertion<TException> : Assertion<TException> where TException : Exception
    {
        private readonly DateTime value;

        internal DateTimeAssertion(DateTime value) : base()
        {
            this.value = value;
        }

        public DateTimeAssertion<TException> IsTrue(Func<DateTime, bool> func, string message)
        {
            if (!func.Invoke(value))
                ThrowException(message);

            return this;
        }

        public DateTimeAssertion<TException> IsFalse(Func<DateTime, bool> func, string message)
        {
            if (func.Invoke(value))
                ThrowException(message);

            return this;
        }

        public DateTimeAssertion<TException> IsDefault(string message)
        {
            if (value != default)
                ThrowException(message);

            return this;
        }

        public DateTimeAssertion<TException> IsNotDefault(string message)
        {
            if (value == default)
                ThrowException(message);

            return this;
        }

        public DateTimeAssertion<TException> IsBetween(DateTime minimum, DateTime maximum, string message)
        {
            if (value < minimum || value > maximum)
                ThrowException(message);

            return this;
        }

        public DateTimeAssertion<TException> IsLessThan(DateTime maximun, string message)
        {
            if (value >= maximun)
                ThrowException(message);

            return this;
        }

        public DateTimeAssertion<TException> IsLessThanOrEqual(DateTime maximun, string message)
        {
            if (value > maximun)
                ThrowException(message);

            return this;
        }

        public DateTimeAssertion<TException> IsGreaterThan(DateTime minimum, string message)
        {
            if (value <= minimum)
                ThrowException(message);

            return this;
        }

        public DateTimeAssertion<TException> IsGreaterThanOrEqual(DateTime minimum, string message)
        {
            if (value < minimum)
                ThrowException(message);

            return this;
        }


    }
}
