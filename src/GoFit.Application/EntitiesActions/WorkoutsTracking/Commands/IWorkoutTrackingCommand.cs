namespace GoFit.Application.EntitiesActions.WorkoutsTracking.Commands;

/// <summary>
/// A command that changes an existing workout tracking on behalf of the calling user.
/// </summary>
public interface IWorkoutTrackingCommand
{
    Guid WorkoutsTrackingId { get; }
    string AppUserId { get; }
}
