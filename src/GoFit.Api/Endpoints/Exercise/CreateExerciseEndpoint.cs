using GoFit.Application.Common;
using GoFit.Application.EntitiesActions.Exercises.Commands;

namespace GoFit.Api.Endpoints.Exercise;

public class CreateExerciseEndpoint :
    BaseEndpoint<CreateExerciseCommand, Guid>
{
    public override void Configure()
    {
        Post("Exercise");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CreateExerciseCommand req, CancellationToken ct)
    {
        Result<Guid> result = await Mediator.Send(req, ct);

        await HandleResultResponse(result, ct);
    }
}
