using GoFit.Application.Common;
using GoFit.Application.EntitiesActions.Exercises.Dtos;
using GoFit.Application.EntitiesActions.Exercises.Queries;

namespace GoFit.Api.Endpoints.Exercise;

public class GetExerciseByIdEndpoint :
    BaseEndpoint<GetExerciseDtoByIdQuery, ExerciseDto?>
{
    public override void Configure()
    {
        Get("Exercise/{id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetExerciseDtoByIdQuery req, CancellationToken ct)
    {
        ValidatorResponse<ExerciseDto?> response = await Mediator.Send(req, ct);

        await ResolveResponseAsync(response, ct);
    }
}
