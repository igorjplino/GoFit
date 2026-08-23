using GoFit.Application.Common;
using GoFit.Application.EntitiesActions.Workouts.Dtos;
using GoFit.Application.EntitiesActions.Workouts.Queries;
using GoFit.Domain.Authorization;

namespace GoFit.Api.Endpoints.Workout;

public class GetWorkoutByIdEndpoint :
    BaseEndpoint<GetWorkoutDtoByIdQuery, WorkoutDto?>
{
    public GetWorkoutByIdEndpoint(ILogger<GetWorkoutByIdEndpoint> logger)
        : base(logger)
    { }

    public override void Configure()
    {
        Get("Workout/{id}");
        Permissions(AppPermissions.Training.ViewWorkouts);
    }

    public override async Task HandleAsync(GetWorkoutDtoByIdQuery req, CancellationToken ct)
    {
        Result<WorkoutDto?> result = await Mediator.Send(req, ct);

        await HandleResultResponse(result, ct);
    }
}
