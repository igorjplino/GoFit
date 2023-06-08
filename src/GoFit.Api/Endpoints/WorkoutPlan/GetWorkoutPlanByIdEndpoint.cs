using FastEndpoints;
using GoFit.Application.WorkoutPlans.Dtos;
using GoFit.Application.WorkoutPlans.Queries;
using MediatR;

namespace GoFit.Api.Endpoints.WorkoutPlan;

public class GetWorkoutPlanByIdEndpoint :
    Endpoint<GetWorkoutPlanDtoByIdQuery, WorkoutPlanDto>
{
    public IMediator Mediator { get; init; }

    public override void Configure()
    {
        Get("WorkoutPlan/{id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetWorkoutPlanDtoByIdQuery req, CancellationToken ct)
    {
        WorkoutPlanDto? result = await Mediator.Send(req);

        if (result is null)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        await SendAsync(result, cancellation: ct);
    }
}
