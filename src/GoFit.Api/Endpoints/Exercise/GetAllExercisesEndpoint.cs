using GoFit.Application.Common;
using GoFit.Application.EntitiesActions.Exercises.Dtos;
using GoFit.Application.EntitiesActions.Exercises.Queries;
using GoFit.Application.Models;

namespace GoFit.Api.Endpoints.Exercise;

public class GetAllExercisesEndpoint :
    BaseEndpoint<GetAllExercisesQuery, PaginatedResult<ExerciseDto>>
{
    public GetAllExercisesEndpoint(ILogger<GetAllExercisesEndpoint> logger)
        : base(logger)
    { }

    public override void Configure()
    {
        Get("Exercise");
    }

    public override async Task HandleAsync(GetAllExercisesQuery req, CancellationToken ct)
    {
        var result = await Mediator.Send(req, ct);

        await HandleResultResponse(result, ct);
    }
}
