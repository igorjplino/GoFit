using GoFit.Application.Common;
using GoFit.Application.Interfaces;
using MediatR;

namespace GoFit.Application.EntitiesActions.Exercises.Commands;

public record DeleteExerciseCommand(Guid ExerciseId) : IRequest<Result<Guid>>
{ }

public class DeleteExerciseCommandHandler : IRequestHandler<DeleteExerciseCommand, Result<Guid>>
{
    private readonly IExerciseRepository _exerciseRepository;

    public DeleteExerciseCommandHandler(IExerciseRepository exerciseRepository)
    {
        _exerciseRepository = exerciseRepository;
    }

    public async Task<Result<Guid>> Handle(DeleteExerciseCommand request, CancellationToken cancellationToken)
    {
        await _exerciseRepository.DeleteAsync(request.ExerciseId);

        return request.ExerciseId;
    }
}
