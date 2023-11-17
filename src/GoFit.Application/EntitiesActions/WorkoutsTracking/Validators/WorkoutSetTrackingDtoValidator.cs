using FluentValidation;
using GoFit.Application.EntitiesActions.WorkoutsTracking.Dtos;

namespace GoFit.Application.EntitiesActions.WorkoutsTracking.Validators;
public class WorkoutSetTrackingDtoValidator : AbstractValidator<WorkoutSetTrackingDto>
{
    public WorkoutSetTrackingDtoValidator()
    {
        RuleFor(x => x.Repetitions)
            .NotEmpty()
            .GreaterThan(0);
    }
}
