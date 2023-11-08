using FluentValidation;
using GoFit.Application.EntitiesActions.WorkoutPlans.Dtos;
using GoFit.Application.Interfaces;
using GoFit.Domain.Entities;

namespace GoFit.Application.EntitiesActions.WorkoutPlans.Validators;

public class WorkoutExerciseDtoValidator : AbstractValidator<WorkoutExerciseDto>
{
    private readonly IExerciseRepository _exerciseRepository;

    public WorkoutExerciseDtoValidator(IExerciseRepository exerciseRepository)
    {
        _exerciseRepository = exerciseRepository;

        RuleFor(x => x.ExerciseId)
            .MustAsync(ExerciseMustExistsAsync);
    }

    private async Task<bool> ExerciseMustExistsAsync(Guid id, CancellationToken ct)
    {
        Exercise? exercise = await _exerciseRepository.GetAsync(id);

        return exercise is not null;
    }
}
