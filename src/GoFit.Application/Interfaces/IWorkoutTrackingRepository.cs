using GoFit.Domain.Entities;

namespace GoFit.Application.Interfaces;

public interface IWorkoutTrackingRepository : IBaseRepository<WorkoutTracking>
{
    Task<WorkoutTracking?> GetWithSetsAsync(Guid id);
    Task UpdateWorkoutTrackingAsync(WorkoutTracking workoutTracking);
}
