namespace OnlineStoreBackend.Abstractions.Models;

public class Result
{
    public string Error { get; }
    public bool IsSuccess => Error is null;

    protected Result()
    {
    }
    protected Result(string error)
        => Error = error;
    
    public static Result Success()
        => new();

    public static Result<T> Success<T>(T value)
        => new(value);

    public static Result Fail(string error)
        => new(error);

    public static Result<T> Fail<T>(string error)
        => new(error);
}

public sealed class Result<T> : Result
{
    public T Value { get; }

    public Result(T value)
        => Value = value;

    public Result(string error) : base(error) { }
}