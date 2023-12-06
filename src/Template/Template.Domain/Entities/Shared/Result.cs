namespace Template.Domain.Entities.Shared
{
    public class Result
    {
        protected internal Result(bool isSucess, Exception? exception)
        {
            if (isSucess && exception != null)
            {
                throw new InvalidOperationException();
            }

            if (!isSucess && exception == null)
            {
                throw new InvalidOperationException();
            }

            IsSuccess = isSucess;
            Exception = exception;

        }

        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;

        public Exception? Exception { get; }

        public static Result Success() => new(true, null);
        public static Result Failure(Exception exception) => new(false, exception);
    }

    public class Result<TValue> : Result
    {
        private readonly TValue? value;

        protected internal Result(TValue? value, bool isSucess, Exception? exception) : base(isSucess, exception)
        {
            this.value = value;
        }

        public TValue Value => IsSuccess ? value : throw new InvalidOperationException("The value of a failure result can not be accessed.");

        public static implicit operator Result<TValue>(TValue? value) => new Result<TValue>(value, true, null);

        public static Result<TValue> Success(TValue value) => new(value, true, null);
        public static new Result<TValue> Success() => new(default, true, null);
        public static Result<TValue> Failure<TException>(TException exception) where TException : Exception
            => new(default, false, exception);
    }
}
