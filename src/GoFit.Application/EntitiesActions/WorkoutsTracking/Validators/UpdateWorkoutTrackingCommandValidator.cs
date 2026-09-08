using FluentValidation;
using GoFit.Application.EntitiesActions.WorkoutsTracking.Commands;
using GoFit.Application.Interfaces;
using GoFit.Domain.Entities;

namespace GoFit.Application.EntitiesActions.WorkoutsTracking.Validators;
public class UpdateWorkoutTrackingCommandValidator : AbstractValidator<UpdateWorkoutTrackingCommand>
{
    private const string AthleteContextKey = "Athlete";
    private const string TrackingContextKey = "WorkoutTracking";

    private readonly IWorkoutTrackingRepository _workoutTrackingRepository;
    private readonly IAthleteRepository _athleteRepository;

    public UpdateWorkoutTrackingCommandValidator(
        IWorkoutTrackingRepository workoutTrackingRepository,
        IAthleteRepository athleteRepository)
    {
        _workoutTrackingRepository = workoutTrackingRepository;
        _athleteRepository = athleteRepository;

        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.Note)
            .MaximumLength(300).When(x => x.Note is not null);

        RuleForEach(x => x.Sets)
            .SetValidator(new WorkoutSetTrackingDtoValidator());

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

    private async Task<bool> HaveLinkedAthlete(UpdateWorkoutTrackingCommand command, string appUserId, ValidationContext<UpdateWorkoutTrackingCommand> context, CancellationToken ct)
        => await GetAthlete(appUserId, context, ct) is not null;

    private async Task<bool> TrackingExists(UpdateWorkoutTrackingCommand command, Guid trackingId, ValidationContext<UpdateWorkoutTrackingCommand> context, CancellationToken ct)
        => await GetTracking(trackingId, context, ct) is not null;

    private async Task EnsureOwnsTracking(UpdateWorkoutTrackingCommand command, ValidationContext<UpdateWorkoutTrackingCommand> context, CancellationToken ct)
    {
        var athlete = await GetAthlete(command.AppUserId, context, ct);
        var tracking = await GetTracking(command.WorkoutsTrackingId, context, ct);

        if (athlete is not null && tracking is not null && tracking.AthleteId != athlete.Id)
            context.AddFailure(nameof(command.WorkoutsTrackingId), "You do not have permission to modify this workout tracking.");
    }

    private async Task EnsureNotAlreadyFinished(UpdateWorkoutTrackingCommand command, ValidationContext<UpdateWorkoutTrackingCommand> context, CancellationToken ct)
    {
        var tracking = await GetTracking(command.WorkoutsTrackingId, context, ct);

        if (tracking?.EndWorkoutDate is not null)
            context.AddFailure(nameof(command.WorkoutsTrackingId), "This workout has already been finished and cannot be modified.");
    }

    private async Task EnsureNotAlreadyCancelled(UpdateWorkoutTrackingCommand command, ValidationContext<UpdateWorkoutTrackingCommand> context, CancellationToken ct)
    {
        var tracking = await GetTracking(command.WorkoutsTrackingId, context, ct);

        if (tracking?.CancelledDate is not null)
            context.AddFailure(nameof(command.WorkoutsTrackingId), "This workout has already been cancelled and cannot be modified.");
    }

    // Cached on the validation context so every rule above shares one fetch per entity, instead of
    // each rule independently re-querying for the same athlete/tracking row.
    private async Task<Athlete?> GetAthlete(string appUserId, ValidationContext<UpdateWorkoutTrackingCommand> context, CancellationToken ct)
    {
        if (context.RootContextData.TryGetValue(AthleteContextKey, out var cached))
            return (Athlete?)cached;

        var athlete = await _athleteRepository.GetByAppUserIdAsync(appUserId);
        context.RootContextData[AthleteContextKey] = athlete!;
        return athlete;
    }

    private async Task<WorkoutTracking?> GetTracking(Guid id, ValidationContext<UpdateWorkoutTrackingCommand> context, CancellationToken ct)
    {
        if (context.RootContextData.TryGetValue(TrackingContextKey, out var cached))
            return (WorkoutTracking?)cached;

        var tracking = await _workoutTrackingRepository.GetAsync(id);
        context.RootContextData[TrackingContextKey] = tracking!;
        return tracking;
    }
}
