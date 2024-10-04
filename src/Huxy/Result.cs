using System;
using System.Collections.Generic;

namespace Huxy
{
    public abstract class Result
    {
        public bool Success { get; protected set; }
        public bool Failure => !Success;
        public string Message { get; protected set; }
        public IReadOnlyCollection<Error> Errors { get; protected set; } = Array.Empty<Error>();
        public static implicit operator bool(Result result) => result.Success;

        public Exception Exception { get; protected set; } = new Exception("Not set");
        public static OkResult Ok() => new OkResult();
        public static OkResult<T> Ok<T>(T data) => new OkResult<T>(data);
        public static ErrorResult Error(string message) => new ErrorResult(message);
        public static ErrorResult Error(string message, IReadOnlyCollection<Error> errors) => new ErrorResult(message, errors);
        public static ErrorResult Error(Result errorResult) => new ErrorResult(errorResult.Message, errorResult.Errors);
        public static ErrorResult Error(Exception exception) => new ErrorResult(exception);
        public static ErrorResult<T> Error<T>(string message) => new ErrorResult<T>(message);
        public static ErrorResult<T> Error<T>(string message, IReadOnlyCollection<Error> errors) => new ErrorResult<T>(message, errors);
        public static ErrorResult<T> Error<T>(Result errorResult) => new ErrorResult<T>(errorResult.Message, errorResult.Errors);
        public static ErrorResult<T> Error<T>(Exception exception) => new ErrorResult<T>(exception);
    }

    public abstract class Result<T> : Result
    {
        private T _data;

        protected Result(T data)
        {
            _data = data;
        }

        public T Data
        {
            get => Success
                ? _data
                : throw new Exception($"You can't access .{nameof(Data)} when .{nameof(Success)} is false");
            protected set => _data = value;
        }

        public static implicit operator bool(Result<T> result) => result.Success;
    }

    public class OkResult : Result, IOkResult
    {
        public OkResult()
        {
            Success = true;
        }
    }

    public class OkResult<T> : Result<T>, IOkResult
    {
        public OkResult(T data) : base(data)
        {
            Success = true;
        }
    }

    public class ErrorResult : Result, IErrorResult
    {
        public ErrorResult(string message) : this(message, Array.Empty<Error>())
        {
        }

        public ErrorResult(Exception exception) : this(exception.Message, Array.Empty<Error>())
        {
            Exception = exception;
        }

        public ErrorResult(string message, IReadOnlyCollection<Error> errors)
        {
            Message = message;
            Success = false;
            Errors = errors ?? Array.Empty<Error>();
        }
    }

    public class ErrorResult<T> : Result<T>, IErrorResult
    {
        public ErrorResult(string message) : this(message, Array.Empty<Error>())
        {
        }
        
        public ErrorResult(Exception exception) : this(exception.Message, Array.Empty<Error>())
        {
            Exception = exception;
        }


        public ErrorResult(string message, IReadOnlyCollection<Error> errors) : base(default(T))
        {
            Message = message;
            Success = false;
            Errors = errors ?? Array.Empty<Error>();
        }
    }

    public class Error
    {
        public Error(string code, string details)
        {
            Code = code;
            Details = details;
        }

        public Error(string details) : this(null, details)
        {
        }

        public string Code { get; }
        public string Details { get; }
    }

    public interface IErrorResult
    {
    }

    public interface IOkResult
    {
    }
}