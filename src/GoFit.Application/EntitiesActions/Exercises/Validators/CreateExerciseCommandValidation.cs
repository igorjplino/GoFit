using FluentValidation;
using GoFit.Application.EntitiesActions.Exercises.Commands;
using GoFit.Application.Interfaces;

namespace GoFit.Application.EntitiesActions.Exercises.Validators;
public class CreateExerciseCommandValidation : AbstractValidator<CreateExerciseCommand>
{
    private readonly IExerciseRepository _exerciseRepository;

    public CreateExerciseCommandValidation(IExerciseRepository exerciseRepository)
    {
        _exerciseRepository = exerciseRepository;

        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.Name)
            .NotNull()
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(100)
            .MustAsync(IsUniqueName).WithMessage("The exercise name already '{PropertyValue}' exists");

        RuleFor(x => x.Description)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(100);
    }

    private async Task<bool> IsUniqueName(string name, CancellationToken ct)
    {
        var exercise = await _exerciseRepository.GetByNameAsync(name);

        return exercise is null;
    }
}
