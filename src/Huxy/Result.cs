using System;

namespace Huxy
{
    public class Result<T>
    {
        public bool Success { get; protected set; }
        public bool Failure => !Success;
        public string Message { get; protected set; }
        public Exception Exception { get; protected set; }

        private T _data;

        public Result(T data, string message = "", Exception exception = null, bool success = true)
        {
            _data = data;
            Success = success;
            Message = message;
            Exception = exception;
        }

        public T Data
        {
            get => Success
                ? _data
                : throw new InvalidOperationException($"You can't access .{nameof(Data)} when .{nameof(Success)} is false");
            protected set => _data = value;
        }

        public static implicit operator bool(Result<T> result) => result.Success;
    }


    public readonly struct None
    {
    }

    public class Result : Result<None>
    {
        public Result(string message = "", Exception exception = null, bool success = true) : base(default, message, exception, success)
        {
        }

        //Ok 
        public static Result Ok() => new Result();
        public static Result<T> Ok<T>(T data) => new Result<T>(data);

        // Generic Errors
        public static Result Nope(string message) => new Result(message, null, false);
        public static Result Nope(string message, Exception exception) => new Result(message, exception, false);
        public static Result Nope(Exception exception) => new Result(exception.Message, exception, false);
        public static Result Nope(Result other) => new Result(other.Message, other.Exception, false);

        // Typed Errors
        public static Result<TE> Nope<TE>(string message) => new Result<TE>(default, message, null, false);
        public static Result<TE> Nope<TE>(string message, Exception exception) => new Result<TE>(default, message, exception, false);
        public static Result<TE> Nope<TE>(Exception exception) => new Result<TE>(default, exception.Message, exception, false);
        public static Result<TE> Nope<TE>(Result<TE> other) => new Result<TE>(default, other.Message, other.Exception, false);
    }
}