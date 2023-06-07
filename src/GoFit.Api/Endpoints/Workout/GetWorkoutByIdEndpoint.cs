using FastEndpoints;
using GoFit.Api.Contracts.Mappers;
using GoFit.Api.Contracts.Requests;
using GoFit.Api.Contracts.Responses;
using GoFit.Application.Workouts.Dtos;
using GoFit.Application.Workouts.Queries;
using MediatR;

namespace GoFit.Api.Endpoints.Workout;

public class GetWorkoutByIdEndpoint :
    Endpoint<WorkoutRequest, WorkoutResponse, WorkoutMapper>
{
    public IMediator Mediator { get; init; }

    public override void Configure()
    {
        Get("Workout/{id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(WorkoutRequest req, CancellationToken ct)
    {
        WorkoutDto? result = await Mediator.Send(new GetWorkoutDtoByIdQuery { Id = req.Id });

        if (result is null)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        await SendAsync(Map.FromEntity(result), cancellation: ct);
    }
}
