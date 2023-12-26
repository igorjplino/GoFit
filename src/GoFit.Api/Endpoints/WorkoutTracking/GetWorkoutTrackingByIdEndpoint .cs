using GoFit.Application.Common;
using GoFit.Application.EntitiesActions.WorkoutsTracking.Dtos;
using GoFit.Application.EntitiesActions.WorkoutsTracking.Queries;

namespace GoFit.Api.Endpoints.WorkoutTracking;

public class GetWorkoutTrackingIdEndpoint :
    BaseEndpoint<GetWorkoutTrackingByIdQuery, WorkoutTrackingDto?>
{
    public GetWorkoutTrackingIdEndpoint(ILogger<GetWorkoutTrackingIdEndpoint> logger)
        : base(logger)
    { }

    public override void Configure()
    {
        Get("WorkoutTracking/{id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetWorkoutTrackingByIdQuery req, CancellationToken ct)
    {
        Result<WorkoutTrackingDto?> result = await Mediator.Send(req, ct);

        await HandleResultResponse(result, ct);
    }
}
