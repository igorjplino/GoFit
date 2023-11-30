using FluentValidation;
using GoFit.Application.Common.Validators;
using GoFit.Application.EntitiesActions.WorkoutsTracking.Commands;
using GoFit.Application.Interfaces;
using GoFit.Domain.Entities;

namespace GoFit.Application.EntitiesActions.WorkoutsTracking.Validators;
public class UpdateWorkoutTrackingCommandValidator : AbstractValidator<UpdateWorkoutTrackingCommand>
{
    public UpdateWorkoutTrackingCommandValidator(IWorkoutTrackingRepository workoutTrackingRepository)
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.WorkoutsTrackingId)
            .SetValidator(new EntityMustExistsValidator<WorkoutTracking>(workoutTrackingRepository));

        RuleFor(x => x.Note)
            .MaximumLength(300).When(x => x.Note is not null);

        RuleFor(x => x.Sets)
            .NotEmpty();

        RuleForEach(x => x.Sets)
            .SetValidator(new WorkoutSetTrackingDtoValidator());
    }
}
