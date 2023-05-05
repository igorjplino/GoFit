using FluentValidation;

namespace GoFit.Application.WorkoutPlans.Commands.Create;

public class CreateWorkoutPlanCommandValidator : AbstractValidator<CreateWorkoutPlanCommand>
{
    public CreateWorkoutPlanCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty();
    }
}
