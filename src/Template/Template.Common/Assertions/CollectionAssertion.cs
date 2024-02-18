using System.Linq.Expressions;
using Template.Common.TypesExtensions;

namespace Template.Common.Assertions
{
    public class CollectionAssertion<T, TException> : Assertion<TException> where TException : Exception
    {
        private readonly ICollection<T> value;

        internal CollectionAssertion(ICollection<T> value) : base()
        {
            this.value = value;
        }

        public CollectionAssertion<T, TException> IsTrue(Func<ICollection<T>, bool> func, string message)
        {
            if (!func.Invoke(value))
                ThrowException(message);

            return this;
        }

        public CollectionAssertion<T, TException> IsFalse(Func<ICollection<T>, bool> func, string message)
        {
            if (func.Invoke(value))
                ThrowException(message);

            return this;
        }

        public CollectionAssertion<T, TException> IsEmpty(string message)
        {
            if (!value.IsNullOrEmpty())
                ThrowException(message);

            return this;
        }

        public CollectionAssertion<T, TException> IsNotEmpty(string message)
        {
            if (value.IsNullOrEmpty())
                ThrowException(message);

            return this;
        }

        public CollectionAssertion<T, TException> All(Expression<Func<T, bool>> expression, string message)
        {
            if (!value.IsNullOrEmpty())
            {
                if (!value.AsQueryable().All(expression))
                    ThrowException(message);
            }

            return this;
        }

        public CollectionAssertion<T, TException> Any(Expression<Func<T, bool>> expression, string message)
        {
            if (!value.IsNullOrEmpty())
            {
                if (!value.AsQueryable().Any(expression))
                    ThrowException(message);
            }

            return this;
        }

        public CollectionAssertion<T, TException> Contains(T item, string message)
        {
            if (value.IsNullOrEmpty() || !value.Contains(item))
                ThrowException(message);

            return this;
        }

        public CollectionAssertion<T, TException> Contains(T item, IEqualityComparer<T> comparer, string message)
        {
            if (value.IsNullOrEmpty() || !value.Contains(item, comparer))
                ThrowException(message);

            return this;
        }

        public CollectionAssertion<T, TException> NotContains(T item, string message)
        {
            if (!value.IsNullOrEmpty())
            {
                if (value.Contains(item))
                    ThrowException(message);
            }

            return this;
        }

        public CollectionAssertion<T, TException> NotContains(T item, IEqualityComparer<T> comparer, string message)
        {
            if (!value.IsNullOrEmpty())
            {
                if (value.Contains(item, comparer))
                    ThrowException(message);
            }

            return this;
        }

    }
}
