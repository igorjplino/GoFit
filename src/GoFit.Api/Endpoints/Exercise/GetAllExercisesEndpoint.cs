using FastEndpoints;
using GoFit.Application.EntitiesActions.Exercises.Dtos;
using GoFit.Application.EntitiesActions.Exercises.Queries;
using MediatR;

namespace GoFit.Api.Endpoints.Exercise;

public class GetAllExercisesEndpoint :
    Endpoint<GetAllExercisesQuery, IEnumerable<ExerciseDto>>
{
    public required IMediator Mediator { get; init; }

    public override void Configure()
    {
        Get("Exercise");
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetAllExercisesQuery req, CancellationToken ct)
    {
        IEnumerable<ExerciseDto> exercises = await Mediator.Send(req, ct);

        await SendAsync(exercises);
    }
}
