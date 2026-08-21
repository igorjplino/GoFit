using FluentValidation;
using GoFit.Application.Common.Validators;
using GoFit.Application.EntitiesActions.Workouts.Commands;
using GoFit.Application.Interfaces;
using GoFit.Domain.Entities;

namespace GoFit.Application.EntitiesActions.Workouts.Validators;
internal class CreateWorkoutCommandValidator : AbstractValidator<CreateWorkoutCommand>
{
    public CreateWorkoutCommandValidator(IWorkoutPlanRepository workoutPlanRepository)
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.WorkoutPlanId)
            .SetValidator(new EntityMustExistsValidator<WorkoutPlan>(workoutPlanRepository));

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
    }
}
