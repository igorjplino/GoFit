using GoFit.Application.Common;
using GoFit.Application.EntitiesActions.WorkoutsTracking.Commands;

namespace GoFit.Api.Endpoints.WorkoutTracking;

public class UpdateWorkoutTrackingEndpoint :
    BaseEndpoint<UpdateWorkoutTrackingCommand, UpdateWorkoutTrackingCommand>
{
    public override void Configure()
    {
        Put("WorkoutTracking/{WorkoutsTrackingId}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(UpdateWorkoutTrackingCommand req, CancellationToken ct)
    {
        Result<UpdateWorkoutTrackingCommand> result = await Mediator.Send(req, ct);

        await HandleResultResponse(result, ct);
    }
}
