using GoFit.Application.Models;
using GoFit.Domain.Entities;

namespace GoFit.Application.Interfaces;

/// <summary>
/// Writes to the sets logged against a workout tracking. Which position a set takes is a rule owned by the
/// commands in <c>EntitiesActions/WorkoutsTracking</c>; this repository only persists what they decide.
/// </summary>
public interface IWorkoutSetTrackingRepository : IBaseRepository<WorkoutSetTracking>
{
    /// <summary>The tracking's logged sets, ordered by position.</summary>
    Task<List<WorkoutSetTracking>> ListByTrackingIdAsync(Guid workoutTrackingId);

    Task UpdateSetAsync(Guid setId, int repetitions, float weight);

    /// <summary>Deletes one set and applies the new positions of the remaining ones, in a single transaction.</summary>
    Task RemoveAndResequenceAsync(Guid setId, IReadOnlyCollection<WorkoutSetPosition> resequencedSets);
}
