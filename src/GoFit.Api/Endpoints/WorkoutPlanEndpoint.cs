using FastEndpoints;
using GoFit.Api.Contracts.Mappers;
using GoFit.Api.Contracts.Requests;
using GoFit.Api.Contracts.Responses;
using GoFit.Application.WorkoutPlans.Dtos;
using GoFit.Application.WorkoutPlans.Queries;
using MediatR;

namespace GoFit.Api.Endpoints;

public class WorkoutPlanEndpoint :
    Endpoint<WorkoutPlanRequest, WorkoutPlanResponse, WorkoutPlanMapper>
{
    public IMediator Mediator { get; init; }

    public override void Configure()
    {
        Verbs(Http.GET);
        Get("WorkoutPlan/{id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(WorkoutPlanRequest req, CancellationToken ct)
    {
        WorkoutPlanDto? result = await Mediator.Send(new GetWorkoutPlanDtoByIdQuery { Id = req.Id });

        if (result is null)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        await SendAsync(Map.FromEntity(result), cancellation: ct);
    }
}
