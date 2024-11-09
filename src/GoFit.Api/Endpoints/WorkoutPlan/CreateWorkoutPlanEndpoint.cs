using GoFit.Application.Common;
using GoFit.Application.EntitiesActions.WorkoutPlans.Commands;

namespace GoFit.Api.Endpoints.WorkoutPlan;

public class CreateWorkoutPlanEndpoint :
    BaseEndpoint<CreateWorkoutPlanCommand, Guid>
{
    public CreateWorkoutPlanEndpoint(ILogger<CreateWorkoutPlanEndpoint> logger)
        : base(logger)
    { }

    public override void Configure()
    {
        Post("WorkoutPlan");
    }

    public override async Task HandleAsync(CreateWorkoutPlanCommand req, CancellationToken ct)
    {
        Result<Guid> result = await Mediator.Send(req, ct);

        await HandleResultResponse(result, ct);
    }
}
