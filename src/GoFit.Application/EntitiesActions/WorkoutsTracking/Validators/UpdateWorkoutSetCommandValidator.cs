using FluentValidation;
using GoFit.Application.EntitiesActions.WorkoutsTracking.Commands;
using GoFit.Application.Interfaces;

namespace GoFit.Application.EntitiesActions.WorkoutsTracking.Validators;
public class UpdateWorkoutSetCommandValidator : ActiveWorkoutTrackingCommandValidator<UpdateWorkoutSetCommand>
{
    public UpdateWorkoutSetCommandValidator(
        IWorkoutTrackingRepository workoutTrackingRepository,
        IAthleteRepository athleteRepository)
        : base(workoutTrackingRepository, athleteRepository)
    {
        RuleFor(x => x.Repetitions)
            .GreaterThan(0);

        RuleFor(x => x.Weight)
            .GreaterThanOrEqualTo(0);

        RuleFor(x => x)
            .CustomAsync(EnsureSetExists);
    }

    private async Task EnsureSetExists(UpdateWorkoutSetCommand command, ValidationContext<UpdateWorkoutSetCommand> context, CancellationToken ct)
    {
        var tracking = await GetModifiableTrackingWithSetsAsync(command, context, ct);

        if (tracking is not null && tracking.Sets.All(o => o.Order != command.Order))
            context.AddFailure(nameof(command.Order), "Set not found.");
    }
}
