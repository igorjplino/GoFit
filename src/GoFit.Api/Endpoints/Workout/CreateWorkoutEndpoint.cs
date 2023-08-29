using GoFit.Application.Common;
using GoFit.Application.EntitiesActions.Workouts.Commands;

namespace GoFit.Api.Endpoints.Workout;

public class CreateWorkoutEndpoint :
    BaseEndpoint<CreateWorkoutCommand, Guid>
{
    public override void Configure()
    {
        Post("Workout");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CreateWorkoutCommand req, CancellationToken ct)
    {
        Result<Guid> result = await Mediator.Send(req, ct);

        await HandleResultResponse(result, ct);
    }
}
