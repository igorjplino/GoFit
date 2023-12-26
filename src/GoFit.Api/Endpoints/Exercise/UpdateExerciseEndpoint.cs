using GoFit.Application.Common;
using GoFit.Application.EntitiesActions.Exercises.Commands;

namespace GoFit.Api.Endpoints.Exercise;

public class UpdateExerciseEndpoint :
    BaseEndpoint<UpdateExerciseCommand, UpdateExerciseCommand>
{
    public UpdateExerciseEndpoint(ILogger<UpdateExerciseEndpoint> logger)
        : base(logger)
    { }

    public override void Configure()
    {
        Put("Exercise/{ExerciseId}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(UpdateExerciseCommand req, CancellationToken ct)
    {
        Result<UpdateExerciseCommand> result = await Mediator.Send(req, ct);

        await HandleResultResponse(result, ct);
    }
}
