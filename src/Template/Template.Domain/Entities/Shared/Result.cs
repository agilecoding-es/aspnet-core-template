using System;
using System.Text.Json.Serialization;

namespace Template.Domain.Entities.Shared
{
    public class Result
    {
        public Result() { }

        public bool IsSuccess => true;
        public bool IsFailure => !IsSuccess;

        public static Result Success() => new();
        public static Failure Failure(Exception exception) => new(exception);
    }

    public class Failure : Result
    {
        public Failure() { }

        protected internal Failure(Exception exception) : base()
        {
            Exception = exception;
        }

        public new bool IsSuccess => false;

        public Exception Exception { get; }
    }

    public abstract class ResultWithValue<T> : Result
    {
        public ResultWithValue() { }

        protected internal ResultWithValue(T value) : base()
        {
            Value = value;
        }

        [JsonInclude]
        public virtual T Value { get; protected init; }

        public Exception Exception { get; protected set; }

    }

    public class Result<T> : ResultWithValue<T>
    {
        public Result() { }

        protected internal Result(T value) : base(value) { }

        public static Result<T> Success(T value) => new(value);
        public static new Failure<T> Failure(Exception exception) => new(exception);

    }

    public class Failure<T> : Result<T>
    {
        public Failure() { }

        protected internal Failure(Exception exception) : base(default)
        {
            Exception = exception;
        }

        [JsonIgnore]
        public override sealed T Value => throw new InvalidOperationException("The value of a failure result can not be accessed.");

        public new bool IsSuccess => false;

    }
}
