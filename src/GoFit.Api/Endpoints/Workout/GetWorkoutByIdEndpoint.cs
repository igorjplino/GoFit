using FastEndpoints;
using GoFit.Application.EntitiesActions.Workouts.Dtos;
using GoFit.Application.EntitiesActions.Workouts.Queries;
using MediatR;

namespace GoFit.Api.Endpoints.Workout;

public class GetWorkoutByIdEndpoint :
    Endpoint<GetWorkoutDtoByIdQuery, WorkoutDto>
{
    public required IMediator Mediator { get; init; }

    public override void Configure()
    {
        Get("Workout/{id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetWorkoutDtoByIdQuery req, CancellationToken ct)
    {
        WorkoutDto? result = await Mediator.Send(req, ct);

        if (result is null)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        await SendAsync(result, cancellation: ct);
    }
}
