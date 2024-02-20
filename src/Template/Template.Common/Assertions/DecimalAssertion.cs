using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template.Common.Assertions
{
    public class DecimalAssertion<TException> : Assertion<TException> where TException : Exception
    {
        private readonly decimal value;

        internal DecimalAssertion(decimal value) : base()
        {
            this.value = value;
        }
        
        public DecimalAssertion<TException> IsTrue(Func<decimal, bool> func, string message)
        {
            if (!func.Invoke(value))
                ThrowException(message);

            return this;
        }

        public DecimalAssertion<TException> IsFalse(Func<decimal, bool> func, string message)
        {
            if (func.Invoke(value))
                ThrowException(message);

            return this;
        }

        public DecimalAssertion<TException> IsDefault(string message)
        {
            if (value != default)
                ThrowException(message);

            return this;
        }

        public DecimalAssertion<TException> IsNotDefault(string message)
        {
            if (value == default)
                ThrowException(message);

            return this;
        }

        public DecimalAssertion<TException> IsBetween(decimal minimum, decimal maximum, string message)
        {
            if (value < minimum || value > maximum)
                ThrowException(message);

            return this;
        }

        public DecimalAssertion<TException> IsLessThan(decimal maximun, string message)
        {
            if (value >= maximun)
                ThrowException(message);

            return this;
        }

        public DecimalAssertion<TException> IsLessThanOrEqual(decimal maximun, string message)
        {
            if (value > maximun)
                ThrowException(message);

            return this;
        }

        public DecimalAssertion<TException> IsGreaterThan(decimal minimum, string message)
        {
            if (value <= minimum)
                ThrowException(message);

            return this;
        }

        public DecimalAssertion<TException> IsGreaterThanOrEqual(decimal minimum, string message)
        {
            if (value < minimum)
                ThrowException(message);

            return this;
        }


    }
}
