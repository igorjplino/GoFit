using GoFit.Application.Common;
using GoFit.Application.Interfaces;
using GoFit.Domain.Entities;
using MediatR;

namespace GoFit.Application.EntitiesActions.Exercises.Commands;
public record UpdateExerciseCommand(
    Guid ExerciseId,
    string Name,
    string? Description)
    : IRequest<Result<UpdateExerciseCommand>>
{ }

public class UpdateExerciseCommandHandler : IRequestHandler<UpdateExerciseCommand, Result<UpdateExerciseCommand>>
{
    private readonly IExerciseRepository _exerciseRepository;

    public UpdateExerciseCommandHandler(IExerciseRepository exerciseRepository)
    {
        _exerciseRepository = exerciseRepository;
    }

    public async Task<Result<UpdateExerciseCommand>> Handle(UpdateExerciseCommand request, CancellationToken cancellationToken)
    {
        var exercise = new Exercise
        {
            Id = request.ExerciseId,
            Name = request.Name,
            Description = request.Description
        };

        await _exerciseRepository.UpdateAsync(exercise);

        return request;
    }
}
