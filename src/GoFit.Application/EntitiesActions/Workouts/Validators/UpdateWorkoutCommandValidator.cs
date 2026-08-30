using FluentValidation;
using GoFit.Application.EntitiesActions.WorkoutPlans.Validators;
using GoFit.Application.EntitiesActions.Workouts.Commands;
using GoFit.Application.Interfaces;

namespace GoFit.Application.EntitiesActions.Workouts.Validators;

public class UpdateWorkoutCommandValidator : AbstractValidator<UpdateWorkoutCommand>
{
    public UpdateWorkoutCommandValidator(IExerciseRepository exerciseRepository)
    {
        RuleFor(x => x.AppUserId)
            .NotEmpty();

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

        RuleFor(x => x.WorkoutExercises)
            .NotEmpty();

        RuleForEach(x => x.WorkoutExercises)
            .SetValidator(new WorkoutExerciseDtoValidator(exerciseRepository));
    }
}
