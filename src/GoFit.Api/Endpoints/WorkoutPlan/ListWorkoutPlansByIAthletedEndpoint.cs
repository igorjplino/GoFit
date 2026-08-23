using GoFit.Application.Common;
using GoFit.Application.EntitiesActions.WorkoutPlans.Dtos;
using GoFit.Application.EntitiesActions.WorkoutPlans.Queries;
using GoFit.Domain.Authorization;

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
        Permissions(AppPermissions.Training.ViewWorkoutPlans);
    }

    public override async Task HandleAsync(ListWorkoutPlansByIAthletedQuery req, CancellationToken ct)
    {
        Result<List<WorkoutPlanDto>> result = await Mediator.Send(req, ct);

        await HandleResultResponse(result, ct);
    }
}
