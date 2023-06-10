using FastEndpoints;
using GoFit.Application.EntitiesActions.Exercises.Commands;
using MediatR;

namespace GoFit.Api.Endpoints.Exercise;

public class CreateExerciseEndpoint :
    Endpoint<CreateExerciseCommand, Guid>
{
    public required IMediator Mediator { get; init; }

    public override void Configure()
    {
        Post("Exercise");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CreateExerciseCommand req, CancellationToken ct)
    {
        Guid id = await Mediator.Send(req, ct);

        await SendAsync(id, cancellation: ct);
    }
}
