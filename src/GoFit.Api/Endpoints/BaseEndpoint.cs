using FastEndpoints;
using GoFit.Application.Common;
using MediatR;

namespace GoFit.Api.Endpoints;

public abstract class BaseEndpoint<TRequest, TResponse> 
    : Endpoint<TRequest, TResponse>
     where TRequest : notnull
{
    public required IMediator Mediator { get; init; }

    public async Task ResolveResponseAsync(ValidatorResponse<TResponse> response, CancellationToken ct)
    {
        if (response.IsValidResponse)
        {
            await SendAsync(response.Result, cancellation: ct);
            return;
        }

        ValidationFailures.AddRange(response.Erros);

        await SendErrorsAsync(cancellation: ct);
    }
}
