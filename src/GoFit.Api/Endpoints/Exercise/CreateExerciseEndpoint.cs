using GoFit.Application.Common;
using GoFit.Application.EntitiesActions.Exercises.Commands;

namespace GoFit.Api.Endpoints.Exercise;

public class CreateExerciseEndpoint :
    BaseEndpoint<CreateExerciseCommand, Guid>
{
    public CreateExerciseEndpoint(ILogger<CreateExerciseEndpoint> logger)
        : base(logger)
    { }

    public override void Configure()
    {
        Post("Exercise");
    }

    public override async Task HandleAsync(CreateExerciseCommand req, CancellationToken ct)
    {
        Result<Guid> result = await Mediator.Send(req, ct);

        await HandleResultResponse(result, ct);
    }
}
