using FluentValidation;
using GoFit.Application.Common.Validators;
using GoFit.Application.EntitiesActions.WorkoutsTracking.Commands;
using GoFit.Application.Interfaces;
using GoFit.Domain.Entities;

namespace GoFit.Application.EntitiesActions.WorkoutsTracking.Validators;
public class CreateWorkoutTrackingCommandValidator : AbstractValidator<CreateWorkoutTrackingCommand>
{
    public CreateWorkoutTrackingCommandValidator(IWorkoutRepository workoutRepository)
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.WorkoutId)
            .SetValidator(new EntityMustExistsValidator<Workout>(workoutRepository));

        RuleFor(x => x.Note)
            .MaximumLength(300).When(x => x.Note is not null);

        RuleForEach(x => x.Sets)
            .SetValidator(new WorkoutSetTrackingDtoValidator());
    }
}
