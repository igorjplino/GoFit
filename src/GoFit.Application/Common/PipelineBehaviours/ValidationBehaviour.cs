using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace GoFit.Application.Common.PipelineBehaviours;
public class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, Result<TResponse>>
    where TRequest : notnull
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<Result<TResponse>> Handle(
        TRequest request,
        RequestHandlerDelegate<Result<TResponse>> next,
        CancellationToken cancellationToken)
    {
        if (_validators.Any())
        {
            var context = new ValidationContext<TRequest>(request);

            var validationResults = await Task.WhenAll(
                _validators.Select(x => x.ValidateAsync(context, cancellationToken)));

            List<ValidationFailure> failures = validationResults
                .Where(x => x.Errors.Any())
                .SelectMany(x => x.Errors)
                .ToList();

            if (failures.Any())
                return new ValidationException(failures);
        }

        return await next();
    }
}
