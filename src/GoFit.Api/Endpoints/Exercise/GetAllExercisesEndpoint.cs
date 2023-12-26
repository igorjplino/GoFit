using GoFit.Application.Common;
using GoFit.Application.EntitiesActions.Exercises.Dtos;
using GoFit.Application.EntitiesActions.Exercises.Queries;

namespace GoFit.Api.Endpoints.Exercise;

public class GetAllExercisesEndpoint :
    BaseEndpoint<GetAllExercisesQuery, List<ExerciseDto>>
{
    public GetAllExercisesEndpoint(ILogger<GetAllExercisesEndpoint> logger)
        : base(logger)
    { }

    public override void Configure()
    {
        Get("Exercise");
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetAllExercisesQuery req, CancellationToken ct)
    {
        Result<List<ExerciseDto>> result = await Mediator.Send(req, ct);

        await HandleResultResponse(result, ct);
    }
}
