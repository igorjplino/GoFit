using GoFit.Application.Common;
using GoFit.Application.EntitiesActions.WorkoutPlans.Dtos;
using GoFit.Application.EntitiesActions.WorkoutPlans.Queries;

namespace GoFit.Api.Endpoints.WorkoutPlan;

public class ListWorkoutPlansByIAthletedEndpoint :
    BaseEndpoint<ListWorkoutPlansByIAthletedQuery, List<WorkoutPlanDto>>
{
    public ListWorkoutPlansByIAthletedEndpoint(ILogger<ListWorkoutPlansByIAthletedEndpoint> logger)
        : base(logger)
    { }

    public override void Configure()
    {
        Get("WorkoutPlan/Athlete/{id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(ListWorkoutPlansByIAthletedQuery req, CancellationToken ct)
    {
        Result<List<WorkoutPlanDto>> result = await Mediator.Send(req, ct);

        await HandleResultResponse(result, ct);
    }
}
