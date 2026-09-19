using GoFit.Application.EntitiesActions.WorkoutsTracking.Commands;
using GoFit.Application.Interfaces;

namespace GoFit.Application.EntitiesActions.WorkoutsTracking.Validators;
public class CancelWorkoutTrackingCommandValidator : ActiveWorkoutTrackingCommandValidator<CancelWorkoutTrackingCommand>
{
    public CancelWorkoutTrackingCommandValidator(
        IWorkoutTrackingRepository workoutTrackingRepository,
        IAthleteRepository athleteRepository)
        : base(workoutTrackingRepository, athleteRepository)
    { }

    protected override string AlreadyFinishedMessage => "This workout has already been finished and cannot be cancelled.";

    protected override string AlreadyCancelledMessage => "This workout has already been cancelled.";
}
