using FluentValidation;
using GoFit.Application.EntitiesActions.WorkoutsTracking.Commands;
using GoFit.Application.Interfaces;

namespace GoFit.Application.EntitiesActions.WorkoutsTracking.Validators;
public class LogWorkoutSetCommandValidator : ActiveWorkoutTrackingCommandValidator<LogWorkoutSetCommand>
{
    private readonly IWorkoutRepository _workoutRepository;

    public LogWorkoutSetCommandValidator(
        IWorkoutRepository workoutRepository,
        IWorkoutTrackingRepository workoutTrackingRepository,
        IAthleteRepository athleteRepository)
        : base(workoutTrackingRepository, athleteRepository)
    {
        _workoutRepository = workoutRepository;

        RuleFor(x => x.Repetitions)
            .GreaterThan(0);

        RuleFor(x => x.Weight)
            .GreaterThanOrEqualTo(0);

        RuleFor(x => x)
            .CustomAsync(EnsurePlannedSetRemains);
    }

    // Every logged set pairs with the planned set at the same position, so a set beyond the plan has nothing to pair with.
    private async Task EnsurePlannedSetRemains(LogWorkoutSetCommand command, ValidationContext<LogWorkoutSetCommand> context, CancellationToken ct)
    {
        var tracking = await GetModifiableTrackingWithSetsAsync(command, context, ct);
        if (tracking is null)
            return;

        var workout = await _workoutRepository.GetWithDetailsAsync(tracking.WorkoutId);
        var plannedSets = workout?.WorkoutExercises.Sum(o => o.Sets.Count) ?? 0;

        if (tracking.Sets.Count >= plannedSets)
            context.AddFailure(nameof(command.WorkoutsTrackingId), "All planned sets have already been logged.");
    }
}
