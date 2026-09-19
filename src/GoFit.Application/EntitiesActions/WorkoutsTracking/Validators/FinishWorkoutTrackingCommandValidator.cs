using GoFit.Application.EntitiesActions.WorkoutsTracking.Commands;
using GoFit.Application.Interfaces;

namespace GoFit.Application.EntitiesActions.WorkoutsTracking.Validators;
public class FinishWorkoutTrackingCommandValidator : ActiveWorkoutTrackingCommandValidator<FinishWorkoutTrackingCommand>
{
    public FinishWorkoutTrackingCommandValidator(
        IWorkoutTrackingRepository workoutTrackingRepository,
        IAthleteRepository athleteRepository)
        : base(workoutTrackingRepository, athleteRepository)
    { }

    protected override string AlreadyFinishedMessage => "This workout has already been finished.";

    protected override string AlreadyCancelledMessage => "This workout has already been cancelled and cannot be finished.";
}
