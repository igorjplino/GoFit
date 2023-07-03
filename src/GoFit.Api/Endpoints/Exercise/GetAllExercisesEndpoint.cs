using GoFit.Application.Common;
using GoFit.Application.EntitiesActions.Exercises.Dtos;
using GoFit.Application.EntitiesActions.Exercises.Queries;

namespace GoFit.Api.Endpoints.Exercise;

public class GetAllExercisesEndpoint :
    BaseEndpoint<GetAllExercisesQuery, IEnumerable<ExerciseDto>>
{
    public override void Configure()
    {
        Get("Exercise");
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetAllExercisesQuery req, CancellationToken ct)
    {
        ValidatorResponse<IEnumerable<ExerciseDto>> response = await Mediator.Send(req, ct);

        await ResolveResponseAsync(response, ct);
    }
}
