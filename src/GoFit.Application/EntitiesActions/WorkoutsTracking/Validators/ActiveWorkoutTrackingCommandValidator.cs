using FluentValidation;
using GoFit.Application.EntitiesActions.WorkoutsTracking.Commands;
using GoFit.Application.Interfaces;
using GoFit.Domain.Entities;

namespace GoFit.Application.EntitiesActions.WorkoutsTracking.Validators;

/// <summary>
/// Rules shared by every command that changes an existing workout tracking: the caller has a linked athlete,
/// the tracking exists and belongs to that athlete, and it is still in progress (neither finished nor cancelled).
/// </summary>
public abstract class ActiveWorkoutTrackingCommandValidator<TCommand> : AbstractValidator<TCommand>
    where TCommand : IWorkoutTrackingCommand
{
    private const string AthleteContextKey = "Athlete";
    private const string TrackingContextKey = "WorkoutTracking";
    private const string TrackingWithSetsContextKey = "WorkoutTrackingWithSets";

    private readonly IWorkoutTrackingRepository _workoutTrackingRepository;
    private readonly IAthleteRepository _athleteRepository;

    protected ActiveWorkoutTrackingCommandValidator(
        IWorkoutTrackingRepository workoutTrackingRepository,
        IAthleteRepository athleteRepository)
    {
        _workoutTrackingRepository = workoutTrackingRepository;
        _athleteRepository = athleteRepository;

        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.AppUserId)
            .NotEmpty()
            .MustAsync(HaveLinkedAthlete).WithMessage("No athlete is linked to the current account.");

        RuleFor(x => x.WorkoutsTrackingId)
            .MustAsync(TrackingExists).WithMessage("Workout tracking not found.");

        RuleFor(x => x)
            .CustomAsync(EnsureOwnsTracking);

        RuleFor(x => x)
            .CustomAsync(EnsureNotAlreadyFinished);

        RuleFor(x => x)
            .CustomAsync(EnsureNotAlreadyCancelled);
    }

    protected virtual string AlreadyFinishedMessage => "This workout has already been finished and cannot be modified.";

    protected virtual string AlreadyCancelledMessage => "This workout has already been cancelled and cannot be modified.";

    /// <summary>
    /// The tracking with its logged sets, or null when any of the shared rules fails. That failure is already
    /// reported, so a derived rule should skip its own check instead of adding a second, misleading error.
    /// </summary>
    protected async Task<WorkoutTracking?> GetModifiableTrackingWithSetsAsync(TCommand command, ValidationContext<TCommand> context, CancellationToken ct)
    {
        var athlete = await GetAthlete(command.AppUserId, context, ct);
        var tracking = await GetTracking(command.WorkoutsTrackingId, context, ct);

        if (athlete is null || tracking is null || tracking.AthleteId != athlete.Id
            || tracking.EndWorkoutDate is not null || tracking.CancelledDate is not null)
        {
            return null;
        }

        if (context.RootContextData.TryGetValue(TrackingWithSetsContextKey, out var cached))
        {
            return (WorkoutTracking?)cached;
        }

        var trackingWithSets = await _workoutTrackingRepository.GetWithSetsAsync(command.WorkoutsTrackingId);
        context.RootContextData[TrackingWithSetsContextKey] = trackingWithSets!;
        return trackingWithSets;
    }

    private async Task<bool> HaveLinkedAthlete(TCommand command, string appUserId, ValidationContext<TCommand> context, CancellationToken ct)
        => await GetAthlete(appUserId, context, ct) is not null;

    private async Task<bool> TrackingExists(TCommand command, Guid trackingId, ValidationContext<TCommand> context, CancellationToken ct)
        => await GetTracking(trackingId, context, ct) is not null;

    private async Task EnsureOwnsTracking(TCommand command, ValidationContext<TCommand> context, CancellationToken ct)
    {
        var athlete = await GetAthlete(command.AppUserId, context, ct);
        var tracking = await GetTracking(command.WorkoutsTrackingId, context, ct);

        if (athlete is not null && tracking is not null && tracking.AthleteId != athlete.Id)
            context.AddFailure(nameof(command.WorkoutsTrackingId), "You do not have permission to modify this workout tracking.");
    }

    private async Task EnsureNotAlreadyFinished(TCommand command, ValidationContext<TCommand> context, CancellationToken ct)
    {
        var tracking = await GetTracking(command.WorkoutsTrackingId, context, ct);

        if (tracking?.EndWorkoutDate is not null)
            context.AddFailure(nameof(command.WorkoutsTrackingId), AlreadyFinishedMessage);
    }

    private async Task EnsureNotAlreadyCancelled(TCommand command, ValidationContext<TCommand> context, CancellationToken ct)
    {
        var tracking = await GetTracking(command.WorkoutsTrackingId, context, ct);

        if (tracking?.CancelledDate is not null)
            context.AddFailure(nameof(command.WorkoutsTrackingId), AlreadyCancelledMessage);
    }

    // Cached on the validation context so every rule above shares one fetch per entity, instead of
    // each rule independently re-querying for the same athlete/tracking row.
    private async Task<Athlete?> GetAthlete(string appUserId, ValidationContext<TCommand> context, CancellationToken ct)
    {
        if (context.RootContextData.TryGetValue(AthleteContextKey, out var cached))
            return (Athlete?)cached;

        var athlete = await _athleteRepository.GetByAppUserIdAsync(appUserId);
        context.RootContextData[AthleteContextKey] = athlete!;
        return athlete;
    }

    private async Task<WorkoutTracking?> GetTracking(Guid id, ValidationContext<TCommand> context, CancellationToken ct)
    {
        if (context.RootContextData.TryGetValue(TrackingContextKey, out var cached))
            return (WorkoutTracking?)cached;

        var tracking = await _workoutTrackingRepository.GetAsync(id);
        context.RootContextData[TrackingContextKey] = tracking!;
        return tracking;
    }
}
