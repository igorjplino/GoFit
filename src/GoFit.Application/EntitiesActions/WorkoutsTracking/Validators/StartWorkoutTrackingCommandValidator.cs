using FluentValidation;
using GoFit.Application.Common.Validators;
using GoFit.Application.EntitiesActions.WorkoutsTracking.Commands;
using GoFit.Application.Interfaces;
using GoFit.Domain.Entities;

namespace GoFit.Application.EntitiesActions.WorkoutsTracking.Validators;
public class StartWorkoutTrackingCommandValidator : AbstractValidator<StartWorkoutTrackingCommand>
{
    private const string AthleteContextKey = "Athlete";

    private readonly IWorkoutRepository _workoutRepository;
    private readonly IWorkoutTrackingRepository _workoutTrackingRepository;
    private readonly IAthleteRepository _athleteRepository;

    public StartWorkoutTrackingCommandValidator(
        IWorkoutRepository workoutRepository,
        IWorkoutTrackingRepository workoutTrackingRepository,
        IAthleteRepository athleteRepository)
    {
        _workoutRepository = workoutRepository;
        _workoutTrackingRepository = workoutTrackingRepository;
        _athleteRepository = athleteRepository;

        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.WorkoutId)
            .SetValidator(new EntityMustExistsValidator<Workout>(workoutRepository));

        RuleFor(x => x.Note)
            .MaximumLength(300).When(x => x.Note is not null);

        RuleFor(x => x.AppUserId)
            .NotEmpty()
            .MustAsync(HaveLinkedAthlete).WithMessage("No athlete is linked to the current account.");

        RuleFor(x => x)
            .CustomAsync(EnsureOwnsWorkout);

        RuleFor(x => x)
            .CustomAsync(EnsureNoActiveWorkout);
    }

    private async Task<bool> HaveLinkedAthlete(StartWorkoutTrackingCommand command, string appUserId, ValidationContext<StartWorkoutTrackingCommand> context, CancellationToken ct)
        => await GetAthlete(appUserId, context, ct) is not null;

    private async Task EnsureOwnsWorkout(StartWorkoutTrackingCommand command, ValidationContext<StartWorkoutTrackingCommand> context, CancellationToken ct)
    {
        var athlete = await GetAthlete(command.AppUserId, context, ct);
        if (athlete is null)
            return;

        var workout = await _workoutRepository.GetWithDetailsAsync(command.WorkoutId);
        if (workout is not null && workout.WorkoutPlan.AthleteId != athlete.Id)
            context.AddFailure(nameof(command.WorkoutId), "You do not have permission to start this workout.");
    }

    private async Task EnsureNoActiveWorkout(StartWorkoutTrackingCommand command, ValidationContext<StartWorkoutTrackingCommand> context, CancellationToken ct)
    {
        var athlete = await GetAthlete(command.AppUserId, context, ct);
        if (athlete is null)
            return;

        var active = await _workoutTrackingRepository.GetActiveByAthleteIdAsync(athlete.Id);
        if (active is not null)
            context.AddFailure(nameof(command.WorkoutId), "You already have an active workout in progress. Finish or cancel it before starting a new one.");
    }

    // Cached on the validation context so every rule above shares one fetch, instead of each rule
    // independently re-querying for the same athlete. Workout/active-workout each have only one call
    // site today, so they're left as direct calls - no caching wrapper needed for a single fetch.
    private async Task<Athlete?> GetAthlete(string appUserId, ValidationContext<StartWorkoutTrackingCommand> context, CancellationToken ct)
    {
        if (context.RootContextData.TryGetValue(AthleteContextKey, out var cached))
            return (Athlete?)cached;

        var athlete = await _athleteRepository.GetByAppUserIdAsync(appUserId);
        context.RootContextData[AthleteContextKey] = athlete!;
        return athlete;
    }
}
