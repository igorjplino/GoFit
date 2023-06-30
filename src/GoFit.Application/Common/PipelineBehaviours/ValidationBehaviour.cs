using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace GoFit.Application.Common.PipelineBehaviours;
public class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
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
                return ResolveResponseError(failures);
        }

        return await next();
    }

    private static TResponse ResolveResponseError(List<ValidationFailure> failures)
    {
        var resultType = typeof(TResponse).GetGenericArguments()[0];

        var invalidResponse = typeof(ValidatorResponse<>).MakeGenericType(resultType);

        object? instance = Activator.CreateInstance(invalidResponse, null, failures);

        if (instance is not null && instance is TResponse responseResult)
            return responseResult;

        throw new NotImplementedException("Response not implemented corrected");
    }
}
