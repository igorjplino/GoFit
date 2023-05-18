using FastEndpoints;
using GoFit.Api.Contracts.Mappers;
using GoFit.Api.Contracts.Requests;
using GoFit.Api.Contracts.Responses;
using GoFit.Application.Workouts.Dtos;
using GoFit.Application.Workouts.Queries;
using MediatR;

namespace GoFit.Api.Endpoints;

public class WorkoutEndpoint :
    Endpoint<WorkoutRequest, WorkoutResponse, WorkoutMapper>
{
    public IMediator Mediator { get; init; }

    public override void Configure()
    {
        Verbs(Http.GET);
        Get("Workout/{id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(WorkoutRequest req, CancellationToken ct)
    {
        WorkoutDto? result = await Mediator.Send(new GetWorkoutDtoByIdQuery { Id = req.Id });

        var response = Map.FromEntity(result);

        await SendAsync(response, cancellation: ct);
    }
}
