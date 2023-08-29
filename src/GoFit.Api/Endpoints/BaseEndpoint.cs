using FastEndpoints;
using FluentValidation;
using FluentValidation.Results;
using GoFit.Application.Common;
using MediatR;

namespace GoFit.Api.Endpoints;

public abstract class BaseEndpoint<TRequest, TResponse> 
    : Endpoint<TRequest, TResponse?>
     where TRequest : notnull
{
    public required IMediator Mediator { get; init; }

    protected async Task HandleResultResponse(Result<TResponse> result, CancellationToken ct)
    {
        await result.Match(
            async succ => await MapSuccessResponse(succ, ct),
            async fail => await MapFailResponse(fail, ct));
    }

    private async Task MapSuccessResponse(TResponse response, CancellationToken ct)
    {
        if (response is null)
        {
            await SendNotFoundAsync(ct);
        }

        await SendAsync(response, cancellation: ct);
    }

    private async Task MapFailResponse(Exception ex, CancellationToken ct)
    {
        switch (ex)
        {
            case ValidationException validationFailure:
                ValidationFailures.AddRange(validationFailure.Errors);
                break;
            default:
                ValidationFailures.Add(new ValidationFailure
                {
                    ErrorMessage = "An unexpected error occurred",
                });
                break;
        }

        await SendErrorsAsync(cancellation: ct);
    }
}
