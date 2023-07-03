using FastEndpoints;
using GoFit.Application.Common;
using MediatR;

namespace GoFit.Api.Endpoints;

public abstract class BaseEndpoint<TRequest, TResponse> 
    : Endpoint<TRequest, TResponse?>
     where TRequest : notnull
{
    public required IMediator Mediator { get; init; }

    public async Task ResolveResponseAsync(ValidatorResponse<TResponse?> response, CancellationToken ct)
    {
        if (!response.IsValidResponse)
        {
            await ResolveInvalidResponse(response, ct);
            return;
        }

        await SendAsync(response: response.Result, cancellation: ct);
    }

    public async Task ResolveGetByIdResponseAsync(ValidatorResponse<TResponse?> response, CancellationToken ct)
    {
        if (!response.IsValidResponse)
        {
            await ResolveInvalidResponse(response, ct);
            return;
        }

        if (response.Result is null)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        await SendAsync(response: response.Result, cancellation: ct);
    }

    private async Task ResolveInvalidResponse(ValidatorResponse<TResponse?> response, CancellationToken ct)
    {
        ValidationFailures.AddRange(response.Erros);

        await SendErrorsAsync(cancellation: ct);
    }
}
