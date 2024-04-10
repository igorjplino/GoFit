using FluentValidation;
using GoFit.Application.Common.Validators;
using GoFit.Application.EntitiesActions.WorkoutPlans.Commands;
using GoFit.Application.Interfaces;
using GoFit.Domain.Entities;

namespace GoFit.Application.EntitiesActions.WorkoutPlans.Validators;

public class CreateWorkoutPlanCommandValidator : AbstractValidator<CreateWorkoutPlanCommand>
{
    public CreateWorkoutPlanCommandValidator(
        IExerciseRepository exerciseRepository,
        IAthleteRepository athleteRepository)
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.AthleteId)
            .SetValidator(new EntityMustExistsValidator<Athlete>(athleteRepository));

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
