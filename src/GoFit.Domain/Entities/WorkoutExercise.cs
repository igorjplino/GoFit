using GoFit.Domain.Common;

namespace GoFit.Domain.Entities;

public class WorkoutExercise : BaseEntity
{
    public Guid WorkoutId { get; set; }
    public Workout Workout { get; set; }
    public Guid ExerciseId { get; set; }
    public Exercise Exercise { get; set; }
    public int Order { get; set; }
    public ICollection<WorkoutSet> Sets { get; set; } = new List<WorkoutSet>();
}
