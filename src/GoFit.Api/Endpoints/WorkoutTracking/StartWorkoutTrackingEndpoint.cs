using GoFit.Application.Common;
using GoFit.Application.EntitiesActions.WorkoutsTracking.Commands;

namespace GoFit.Api.Endpoints.WorkoutTracking;

public class StartWorkoutTrackingEndpoint :
    BaseEndpoint<StartWorkoutTrackingCommand, Guid>
{
    public StartWorkoutTrackingEndpoint(ILogger<StartWorkoutTrackingEndpoint> logger)
        : base(logger)
    { }

    public override void Configure()
    {
        Post("WorkoutTracking");
    }

    public override async Task HandleAsync(StartWorkoutTrackingCommand req, CancellationToken ct)
    {
        Result<Guid> result = await Mediator.Send(req, ct);

        await HandleResultResponse(result, ct);
    }
}
