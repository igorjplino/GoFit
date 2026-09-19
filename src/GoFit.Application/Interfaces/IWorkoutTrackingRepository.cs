using GoFit.Domain.Entities;

namespace GoFit.Application.Interfaces;

public interface IWorkoutTrackingRepository : IBaseRepository<WorkoutTracking>
{
    Task<WorkoutTracking?> GetWithSetsAsync(Guid id);
    Task<WorkoutTracking?> GetActiveByAthleteIdAsync(Guid athleteId);
    Task CancelWorkoutTrackingAsync(Guid id, DateTime cancelledDate);
    Task FinishWorkoutTrackingAsync(Guid id, DateTime endWorkoutDate);

    /// <summary>
    /// Lists the athlete's workout trackings that are no longer active - finished or cancelled -
    /// most recently started first. The active tracking, if any, is excluded.
    /// </summary>
    Task<List<WorkoutTracking>> ListHistoryByAthleteIdAsync(Guid athleteId);
}
