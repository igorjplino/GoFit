using FastEndpoints;
using GoFit.Application.EntitiesActions.Exercises.Dtos;
using GoFit.Application.EntitiesActions.Exercises.Queries;
using MediatR;

namespace GoFit.Api.Endpoints.Exercise;

public class GetExerciseByIdEndpoint :
    Endpoint<GetExerciseDtoByIdQuery, ExerciseDto>
{
    public IMediator Mediator { get; init; }

    public override void Configure()
    {
        Get("Exercise/{id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetExerciseDtoByIdQuery req, CancellationToken ct)
    {
        ExerciseDto? result = await Mediator.Send(req);

        if (result is null)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        await SendAsync(result, cancellation: ct);
    }
}
