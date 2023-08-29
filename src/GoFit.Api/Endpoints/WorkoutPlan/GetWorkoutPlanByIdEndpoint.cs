using GoFit.Application.Common;
using GoFit.Application.EntitiesActions.WorkoutPlans.Dtos;
using GoFit.Application.EntitiesActions.WorkoutPlans.Queries;

namespace GoFit.Api.Endpoints.WorkoutPlan;

public class GetWorkoutPlanByIdEndpoint :
    BaseEndpoint<GetWorkoutPlanDtoByIdQuery, WorkoutPlanDto?>
{
    public override void Configure()
    {
        Get("WorkoutPlan/{id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetWorkoutPlanDtoByIdQuery req, CancellationToken ct)
    {
        Result<WorkoutPlanDto?> result = await Mediator.Send(req, ct);

        await HandleResultResponse(result, ct);
    }
}
