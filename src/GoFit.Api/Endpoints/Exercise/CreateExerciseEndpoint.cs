using FastEndpoints;
using GoFit.Application.Exercises;
using MediatR;

namespace GoFit.Api.Endpoints.Exercise;

public class CreateExerciseEndpoint :
    Endpoint<CreateExerciseCommand, Guid>
{
    public IMediator Mediator { get; init; }

    public override void Configure()
    {
        Post("Exercise");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CreateExerciseCommand req, CancellationToken ct)
    {
        Guid id = await Mediator.Send(req);

        await SendAsync(id, cancellation: ct);
    }
}
