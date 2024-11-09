using GoFit.Application.Common;
using GoFit.Application.EntitiesActions.WorkoutPlans.Dtos;
using GoFit.Application.EntitiesActions.WorkoutPlans.Queries;

namespace GoFit.Api.Endpoints.WorkoutPlan;

public class GetWorkoutPlanByIdEndpoint :
    BaseEndpoint<GetWorkoutPlanDtoByIdQuery, WorkoutPlanDto?>
{
    public GetWorkoutPlanByIdEndpoint(ILogger<GetWorkoutPlanByIdEndpoint> logger)
        : base(logger)
    { }

    public override void Configure()
    {
        Get("WorkoutPlan/{id}");
    }

    public override async Task HandleAsync(GetWorkoutPlanDtoByIdQuery req, CancellationToken ct)
    {
        Result<WorkoutPlanDto?> result = await Mediator.Send(req, ct);

        await HandleResultResponse(result, ct);
    }
}
