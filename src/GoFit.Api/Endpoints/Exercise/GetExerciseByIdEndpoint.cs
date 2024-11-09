using GoFit.Application.Common;
using GoFit.Application.EntitiesActions.Exercises.Dtos;
using GoFit.Application.EntitiesActions.Exercises.Queries;

namespace GoFit.Api.Endpoints.Exercise;

public class GetExerciseByIdEndpoint :
    BaseEndpoint<GetExerciseDtoByIdQuery, ExerciseDto?>
{
    public GetExerciseByIdEndpoint(ILogger<GetExerciseByIdEndpoint> logger)
        : base(logger)
    { }

    public override void Configure()
    {
        Get("Exercise/{id}");
    }

    public override async Task HandleAsync(GetExerciseDtoByIdQuery req, CancellationToken ct)
    {
        Result<ExerciseDto?> result = await Mediator.Send(req, ct);

        await HandleResultResponse(result, ct);
    }
}
