namespace GoFit.Application.Common;
public readonly struct Result<TValue>
{
    private readonly TValue? _value;
    private readonly Exception? _error;

    private Result(TValue value)
    {
        IsError = false;
        _value = value;
        _error = default;
    }

    private Result(Exception error)
    {
        IsError = true;
        _value = default;
        _error = error;
    }

    public bool IsError { get; }

    public static implicit operator Result<TValue>(TValue value) => new (value);

    public static implicit operator Result<TValue>(Exception error) => new(error);

    public TResult Match<TResult>(
        Func<TValue, TResult> succ,
        Func<Exception, TResult> fail)
    {
        return IsError ? fail(_error!) : succ(_value!);
    }
}
