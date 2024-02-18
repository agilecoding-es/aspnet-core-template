using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template.Common.Assertions
{
    public class FloatAssertion<TException> : Assertion<TException> where TException : Exception
    {
        private readonly float value;

        internal FloatAssertion(float value) : base()
        {
            this.value = value;
        }

        public FloatAssertion<TException> IsTrue(Func<float, bool> func, string message)
        {
            if (!func.Invoke(value))
                ThrowException(message);

            return this;
        }

        public FloatAssertion<TException> IsFalse(Func<float, bool> func, string message)
        {
            if (func.Invoke(value))
                ThrowException(message);

            return this;
        }

        public FloatAssertion<TException> IsDefault(string message)
        {
            if (value != default)
                ThrowException(message);

            return this;
        }

        public FloatAssertion<TException> IsNotDefault(string message)
        {
            if (value == default)
                ThrowException(message);

            return this;
        }

        public FloatAssertion<TException> IsBetween(float minimum, float maximum, string message)
        {
            if (value < minimum || value > maximum)
                ThrowException(message);

            return this;
        }
        
        public FloatAssertion<TException> IsLessThan(float maximun, string message)
        {
            if (value >= maximun)
                ThrowException(message);

            return this;
        }

        public FloatAssertion<TException> IsLessThanOrEqual(float maximun, string message)
        {
            if (value > maximun)
                ThrowException(message);

            return this;
        }

        public FloatAssertion<TException> IsGreaterThan(float minimum, string message)
        {
            if (value <= minimum)
                ThrowException(message);

            return this;
        }

        public FloatAssertion<TException> IsGreaterThanOrEqual(float minimum, string message)
        {
            if (value < minimum)
                ThrowException(message);

            return this;
        }

    }
}
