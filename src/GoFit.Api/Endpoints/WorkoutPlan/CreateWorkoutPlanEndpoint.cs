using FastEndpoints;
using GoFit.Application.EntitiesActions.WorkoutPlans.Commands;
using MediatR;

namespace GoFit.Api.Endpoints.WorkoutPlan;

public class CreateWorkoutPlanEndpoint :
    Endpoint<CreateWorkoutPlanCommand, Guid>
{
    public required IMediator Mediator { get; init; }

    public override void Configure()
    {
        Post("WorkoutPlan");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CreateWorkoutPlanCommand req, CancellationToken ct)
    {
        Guid id = await Mediator.Send(req, ct);

        await SendAsync(id, cancellation: ct);
    }
}
