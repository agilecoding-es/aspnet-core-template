using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template.Common.Assertions
{
    public class ByteExtensions<TException> : Assertion<TException> where TException : Exception
    {
        private readonly byte value;

        internal ByteExtensions(byte value) : base()
        {
            this.value = value;
        }

        public ByteExtensions<TException> IsTrue(Func<int, bool> func, string message)
        {
            if (!func.Invoke(value))
                ThrowException(message);

            return this;
        }

        public ByteExtensions<TException> IsFalse(Func<int, bool> func, string message)
        {
            if (func.Invoke(value))
                ThrowException(message);

            return this;
        }

        public ByteExtensions<TException> IsDefault(string message)
        {
            if (value != default)
                ThrowException(message);

            return this;
        }

        public ByteExtensions<TException> IsNotDefault(string message)
        {
            if (value == default)
                ThrowException(message);

            return this;
        }

        public ByteExtensions<TException> IsBetween(byte minimum, byte maximum, string message)
        {
            if (value < minimum || value > maximum)
                ThrowException(message);

            return this;
        }

        public ByteExtensions<TException> IsLessThan(byte maximun, string message)
        {
            if (value >= maximun)
                ThrowException(message);

            return this;
        }

        public ByteExtensions<TException> IsLessThanOrEqual(byte maximun, string message)
        {
            if (value > maximun)
                ThrowException(message);

            return this;
        }

        public ByteExtensions<TException> IsGreaterThan(byte minimum, string message)
        {
            if (value <= minimum)
                ThrowException(message);

            return this;
        }

        public ByteExtensions<TException> IsGreaterThanOrEqual(byte minimum, string message)
        {
            if (value < minimum)
                ThrowException(message);

            return this;
        }
    }
}
