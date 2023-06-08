using FastEndpoints;
using GoFit.Application.Workouts.Commands;
using MediatR;

namespace GoFit.Api.Endpoints.Workout;

public class CreateWorkoutEndpoint :
    Endpoint<CreateWorkoutCommand, Guid>
{
    public IMediator Mediator { get; init; }

    public override void Configure()
    {
        Post("Workout");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CreateWorkoutCommand req, CancellationToken ct)
    {
        Guid id = await Mediator.Send(req);

        await SendAsync(id, cancellation: ct);
    }
}
