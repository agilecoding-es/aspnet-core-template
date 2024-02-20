using System.Linq.Expressions;
using System.Text.RegularExpressions;
using Template.Common.FunctionalExtensions;

namespace Template.Common.Assertions
{
    public class Assertion<TException> where TException : Exception
    {
        private readonly object value;

        #region Constructors

        protected Assertion()
        { }

        private Assertion(object value)
        {
            this.value = value;
        }

        #endregion

        #region Factory Methods

        public static Assertion<TException> This(object obj) => new Assertion<TException>(obj);

        public static StringAssertion<TException> This(string stringValue) => new StringAssertion<TException>(stringValue);

        public static BoolAssertion<TException> This(bool boolValue) => new BoolAssertion<TException>(boolValue);

        public static CollectionAssertion<T, TException> This<T>(ICollection<T> collectionValue) => new CollectionAssertion<T, TException>(collectionValue);

        #endregion

        #region Public Methods

        public Assertion<TException> IsNull(string message)
        {
            if (value != null)
                ThrowException(message);

            return this;
        }

        public Assertion<TException> IsNotNull(string message)
        {
            if (value == null)
                ThrowException(message);

            return this;
        }

        public Assertion<TException> IsNullOrDefault(string message)
        {
            if (value != null || value != default)
                ThrowException(message);

            return this;
        }

        public Assertion<TException> IsNotNullOrDefault(string message)
        {
            if (value == null || value != default)
                ThrowException(message);

            return this;
        }

        public Assertion<TException> IsOfType<T>(string message)
        {
            if (value != null && value is not T)
                ThrowException(message);

            return this;
        }

        public Assertion<TException> IsNotOfType<T>(string message)
        {
            if (value != null && value is T)
                ThrowException(message);

            return this;
        }

        public Assertion<TException> IsEqual(object object2, string message)
        {
            if (!value.Equals(object2))
                ThrowException(message);

            return this;
        }

        public Assertion<TException> IsNotEqual(object object2, string message)
        {
            if (value.Equals(object2))
                ThrowException(message);

            return this;
        }

        public Assertion<TException> IsEqualOrNull(object object2, string message)
        {
            if (value != null && !value.Equals(object2))
                ThrowException(message);

            return this;
        }

        public Assertion<TException> IsTrue(Func<object, bool> func, string message)
        {
            if (!func.Invoke(value))
                ThrowException(message);

            return this;
        }

        public Assertion<TException> IsFalse(Func<object, bool> func, string message)
        {
            if (func.Invoke(value))
                ThrowException(message);

            return this;
        }

        public Assertion<TException> IsFalse(Func<Assertion<TException>, bool> func, string message)
        {
            if (func.Invoke(this))
                ThrowException(message);

            return this;
        }

        #endregion

        #region Private Methods

        protected Assertion<TException> ThrowException(string message = null)
        {
            var ex = Activator.CreateInstance(typeof(TException), message) as Exception;
            if (ex != null)
                throw ex;

            throw Activator.CreateInstance<TException>();
        }

        #endregion
    }

}
