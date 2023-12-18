using FluentValidation;
using GoFit.Application.EntitiesActions.WorkoutPlans.Dtos;
using GoFit.Application.Interfaces;

namespace GoFit.Application.EntitiesActions.WorkoutPlans.Validators;
public class WorkoutDtoValidator : AbstractValidator<WorkoutDto>
{
    public WorkoutDtoValidator(IExerciseRepository exerciseRepository)
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(100);

        When(x => x.Description is not null, () =>
        {
            RuleFor(x => x.Description)
                .MinimumLength(3)
                .MaximumLength(300);
        });

        RuleForEach(x => x.WorkoutExercises)
            .SetValidator(new WorkoutExerciseDtoValidator(exerciseRepository));
    }
}
