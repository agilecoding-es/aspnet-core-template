using System;
using System.Text.Json.Serialization;

namespace Template.Domain.Entities.Shared
{
    public interface IResult
    {
        bool IsSuccess { get; }
        bool IsFailure { get; }
    }

    public interface IFailure
    {
        Exception Exception { get; }
    }

    public interface IHasValue<T>
    {
        T Value { get; }
    }

    public class Result : IResult
    {
        public Result() { }

        public virtual bool IsSuccess => true;
        public bool IsFailure => !IsSuccess;

        public static Result Success() => new();
        public static Failure Failure(Exception exception) => new(exception);
    }

    public class Failure : Result, IFailure
    {
        public Failure() { }

        protected internal Failure(Exception exception) : base()
        {
            Exception = exception;
        }

        public override bool IsSuccess => false;

        public Exception Exception { get; }
    }

    public class Result<T> : Result, IHasValue<T>
    {
        public Result() { }

        protected internal Result(T value) : base()
        {
            Value = value;
        }

        [JsonInclude]
        public virtual T Value { get; protected init; }

        public static Result<T> Success(T value) => new(value);
        public static new Failure<T> Failure(Exception exception) => new(exception);

    }

    public class Failure<T> : Result<T>, IFailure
    {
        public Failure() { }

        protected internal Failure(Exception exception) : base(default)
        {
            Exception = exception;
        }

        [JsonIgnore]
        public override sealed T Value => throw new InvalidOperationException("The value of a failure result can not be accessed.");

        public override bool IsSuccess => false;

        public Exception Exception { get; }
    }
}
