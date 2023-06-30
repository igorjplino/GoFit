using FluentValidation.Results;
using System.Collections.ObjectModel;

namespace GoFit.Application.Common;

public class ValidatorResponse<TValue>
{
    private readonly IList<ValidationFailure> _errors;

    public ValidatorResponse(TValue value, IList<ValidationFailure>? errors = null)
    {
        Result = value;
        _errors = errors ?? new List<ValidationFailure>();
    }

    public TValue Result { get; }

    public bool IsValidResponse => !_errors.Any();

    public IReadOnlyCollection<ValidationFailure> Erros => new ReadOnlyCollection<ValidationFailure>(_errors);

    public static ValidatorResponse<TValue> Success(TValue value)
    {
        return new ValidatorResponse<TValue>(value);
    }
}