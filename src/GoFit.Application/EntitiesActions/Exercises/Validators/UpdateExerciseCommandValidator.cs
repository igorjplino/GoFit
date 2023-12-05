using FluentValidation;
using GoFit.Application.Common.Validators;
using GoFit.Application.EntitiesActions.Exercises.Commands;
using GoFit.Application.Interfaces;
using GoFit.Domain.Entities;

namespace GoFit.Application.EntitiesActions.Exercises.Validators;
public class UpdateExerciseCommandValidator : AbstractValidator<UpdateExerciseCommand>
{
    private readonly IExerciseRepository _exerciseRepository;

    public UpdateExerciseCommandValidator(IExerciseRepository exerciseRepository)
    {
        _exerciseRepository = exerciseRepository;

        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.ExerciseId)
            .SetValidator(new EntityMustExistsValidator<Exercise>(exerciseRepository));

        RuleFor(x => x.Name)
            .NotNull()
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(100)
            .MustAsync(IsUniqueName).WithMessage("The exercise name '{PropertyValue}' already exists");

        RuleFor(x => x.Description)
            .NotNull()
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(300);
    }

    private async Task<bool> IsUniqueName(string name, CancellationToken ct)
    {
        var exercise = await _exerciseRepository.GetByNameAsync(name);

        return exercise is null;
    }
}
