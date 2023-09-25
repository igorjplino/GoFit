using GoFit.Application.Common;
using GoFit.Application.EntitiesActions.WorkoutsTracking.Commands;

namespace GoFit.Api.Endpoints.WorkoutTracking;

public class CreateWorkoutTrackingEndpoint :
    BaseEndpoint<CreateWorkoutTrackingCommand, Guid>
{
    public override void Configure()
    {
        Post("WorkoutTracking");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CreateWorkoutTrackingCommand req, CancellationToken ct)
    {
        Result<Guid> result = await Mediator.Send(req, ct);

        await HandleResultResponse(result, ct);
    }
}
