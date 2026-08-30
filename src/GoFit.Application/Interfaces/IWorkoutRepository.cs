using GoFit.Domain.Entities;

namespace GoFit.Application.Interfaces;
public interface IWorkoutRepository : IBaseRepository<Workout>
{
    Task<Workout?> GetWithDetailsAsync(Guid id);
    Task UpdateWorkoutAsync(Guid workoutId, string name, string? description, IEnumerable<WorkoutExercise> exercises);
}
