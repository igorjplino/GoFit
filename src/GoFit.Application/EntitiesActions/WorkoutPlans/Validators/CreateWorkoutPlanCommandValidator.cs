using FluentValidation;
using GoFit.Application.EntitiesActions.WorkoutPlans.Commands;
using GoFit.Application.Interfaces;

namespace GoFit.Application.EntitiesActions.WorkoutPlans.Validators;

public class CreateWorkoutPlanCommandValidator : AbstractValidator<CreateWorkoutPlanCommand>
{
    public CreateWorkoutPlanCommandValidator(IExerciseRepository exerciseRepository)
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.Title)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(100);

        When(x => x.Description is not null, () =>
        {
            RuleFor(x => x.Description)
                .MinimumLength(3)
                .MaximumLength(300);
        });

        RuleFor(x => x.Workouts)
            .NotEmpty();

        RuleForEach(x => x.Workouts)
            .NotEmpty()
            .SetValidator(new WorkoutDtoValidator(exerciseRepository));
    }
}
