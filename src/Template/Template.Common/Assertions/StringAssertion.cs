using System.Text.RegularExpressions;

namespace Template.Common.Assertions
{
    public class StringAssertion<TException> : Assertion<TException> where TException : Exception
    {
        private readonly string value;

        internal StringAssertion(string value) : base()
        {
            this.value = value;
        }

        public StringAssertion<TException> IsTrue(Func<string, bool> func, string message)
        {
            if (!func.Invoke(value))
                ThrowException(message);

            return this;
        }

        public StringAssertion<TException> IsFalse(Func<string, bool> func, string message)
        {
            if (func.Invoke(value))
                ThrowException(message);

            return this;
        }

        public StringAssertion<TException> IsNullOrEmpty(string message)
        {
            if (value != null || value.Trim().Length > 0)
                ThrowException(message);

            return this;
        }

        public StringAssertion<TException> IsNotNullOrEmpty(string message)
        {
            if (value == null || value.Trim().Length == 0)
                ThrowException(message);

            return this;
        }

        public StringAssertion<TException> HasLength(int maximum, string message)
        {
            int length = value.Trim().Length;
            if (length > maximum)
                ThrowException(message);

            return this;
        }

        public StringAssertion<TException> HasLength(int minimum, int maximum, string message)
        {
            int length = value.Trim().Length;
            if (length < minimum || length > maximum)
                ThrowException(message);

            return this;
        }


        public StringAssertion<TException> Matches(string pattern, string message)
        {
            var regex = new Regex(pattern);
            if (!regex.IsMatch(value))
                ThrowException(message);

            return this;
        }

    }
}
