using GoFit.Application.Common;
using GoFit.Application.EntitiesActions.Exercises.Commands;
using GoFit.Domain.Authorization;

namespace GoFit.Api.Endpoints.Exercise;

public class DeleteExerciseEndpoint :
    BaseEndpoint<DeleteExerciseCommand, Guid>
{
    public DeleteExerciseEndpoint(ILogger<DeleteExerciseEndpoint> logger)
        : base(logger)
    { }

    public override void Configure()
    {
        Delete("Exercise/{ExerciseId}");
        Permissions(AppPermissions.Exercises.Delete);
    }

    public override async Task HandleAsync(DeleteExerciseCommand req, CancellationToken ct)
    {
        Result<Guid> result = await Mediator.Send(req, ct);

        await HandleResultResponse(result, ct);
    }
}
