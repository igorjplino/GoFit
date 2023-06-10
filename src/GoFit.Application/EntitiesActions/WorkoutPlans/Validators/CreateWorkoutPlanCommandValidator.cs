using FluentValidation;
using GoFit.Application.EntitiesActions.WorkoutPlans.Commands;

namespace GoFit.Application.EntitiesActions.WorkoutPlans.Validators;

public class CreateWorkoutPlanCommandValidator : AbstractValidator<CreateWorkoutPlanCommand>
{
    public CreateWorkoutPlanCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty();
    }
}
