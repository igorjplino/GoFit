using GoFit.Application.Common;
using GoFit.Application.EntitiesActions.WorkoutsTracking.Dtos;
using GoFit.Application.EntitiesActions.WorkoutsTracking.Queries;
using GoFit.Domain.Authorization;

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
        Permissions(AppPermissions.Training.ViewWorkoutTracking);
    }

    public override async Task HandleAsync(GetWorkoutTrackingByIdQuery req, CancellationToken ct)
    {
        Result<WorkoutTrackingDto?> result = await Mediator.Send(req, ct);

        await HandleResultResponse(result, ct);
    }
}
