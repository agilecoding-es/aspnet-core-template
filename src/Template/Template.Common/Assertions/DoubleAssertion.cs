using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template.Common.Assertions
{
    public class DoubleAssertion<TException> : Assertion<TException> where TException : Exception
    {
        private readonly double value;

        internal DoubleAssertion(double value) : base()
        {
            this.value = value;
        }

        public DoubleAssertion<TException> IsTrue(Func<double, bool> func, string message)
        {
            if (!func.Invoke(value))
                ThrowException(message);

            return this;
        }

        public DoubleAssertion<TException> IsFalse(Func<double, bool> func, string message)
        {
            if (func.Invoke(value))
                ThrowException(message);

            return this;
        }

        public DoubleAssertion<TException> IsDefault(string message)
        {
            if (value != default)
                ThrowException(message);

            return this;
        }

        public DoubleAssertion<TException> IsNotDefault(string message)
        {
            if (value == default)
                ThrowException(message);

            return this;
        }

        public DoubleAssertion<TException> IsBetween(double minimum, double maximum, string message)
        {
            if (value < minimum || value > maximum)
                ThrowException(message);

            return this;
        }

        public DoubleAssertion<TException> IsLessThan(double maximun, string message)
        {
            if (value >= maximun)
                ThrowException(message);

            return this;
        }

        public DoubleAssertion<TException> IsLessThanOrEqual(double maximun, string message)
        {
            if (value > maximun)
                ThrowException(message);

            return this;
        }

        public DoubleAssertion<TException> IsGreaterThan(double minimum, string message)
        {
            if (value <= minimum)
                ThrowException(message);

            return this;
        }

        public DoubleAssertion<TException> IsGreaterThanOrEqual(double minimum, string message)
        {
            if (value < minimum)
                ThrowException(message);

            return this;
        }


    }
}
