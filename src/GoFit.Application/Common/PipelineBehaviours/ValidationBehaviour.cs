using FluentValidation;
using FluentValidation.Results;
using GoFit.Application.Extentions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace GoFit.Application.Common.PipelineBehaviours;
public class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, Result<TResponse>>
    where TRequest : notnull
{
    private readonly ILogger<ValidationBehaviour<TRequest, TResponse>> _logger;
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehaviour(
        ILogger<ValidationBehaviour<TRequest, TResponse>> logger,
        IEnumerable<IValidator<TRequest>> validators)
    {
        _logger = logger;
        _validators = validators;
    }

    public async Task<Result<TResponse>> Handle(
        TRequest request,
        RequestHandlerDelegate<Result<TResponse>> next,
        CancellationToken cancellationToken)
    {
        _logger.ExecutingCommand(request.GetType().FullName);

        if (_validators.Any())
        {
            _logger.ValidatingCommand(request.GetType().FullName);

            var context = new ValidationContext<TRequest>(request);

            var validationResults = await Task.WhenAll(
                _validators.Select(x => x.ValidateAsync(context, cancellationToken)));

            List<ValidationFailure> failures = validationResults
                .Where(x => x.Errors.Any())
                .SelectMany(x => x.Errors)
                .ToList();

            if (failures.Count > 0)
            {
                _logger.CommandIsInvalid(request.GetType().FullName, failures.Count);
                return new ValidationException(failures);

            }
        }

        return await next();
    }
}
