using FastEndpoints;
using FluentValidation;
using FluentValidation.Results;
using GoFit.Api.Extensions;
using GoFit.Application.Common;
using MediatR;

namespace GoFit.Api.Endpoints;

public abstract class BaseEndpoint<TRequest, TResponse> 
    : Endpoint<TRequest, TResponse?>
     where TRequest : notnull
{
    protected BaseEndpoint(ILogger<BaseEndpoint<TRequest, TResponse>> logger)
    {
        Logger = logger;
    }

    protected new ILogger<BaseEndpoint<TRequest, TResponse>> Logger { get; }
    public required IMediator Mediator { get; init; }

    /// <summary>
    /// A null success value normally means "the requested resource doesn't exist" (404). Override to
    /// false for endpoints where null means "nothing to report" instead, e.g. "get the current X" queries
    /// where absence is a valid state, not a not-found error - those send 204 No Content instead.
    /// </summary>
    protected virtual bool NullResultIsNotFound => true;

    protected async Task HandleResultResponse(Result<TResponse> result, CancellationToken ct)
    {
        await result.Match(
            async succ => await MapSuccessResponse(succ, ct),
            async fail => await MapFailResponse(fail, ct));
    }

    private async Task MapSuccessResponse(TResponse response, CancellationToken ct)
    {
        if (response is not null)
        {
            await Send.OkAsync(response, cancellation: ct);
        }
        else if (NullResultIsNotFound)
        {
            await Send.NotFoundAsync(ct);
        }
        else
        {
            await Send.NoContentAsync(ct);
        }
    }

    private async Task MapFailResponse(Exception ex, CancellationToken ct)
    {
        switch (ex)
        {
            case ValidationException validationFailure:
                ValidationFailures.AddRange(validationFailure.Errors);
                break;
            default:
                Logger.NotMappedFailResponse(ex);

                ValidationFailures.Add(new ValidationFailure
                {
                    PropertyName = "Unexpected",
                    ErrorMessage = "An unexpected error occurred"
                });
                break;
        }

        await Send.ErrorsAsync(cancellation: ct);
    }
}
