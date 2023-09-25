using GoFit.Domain.Common;

namespace GoFit.Domain.Entities;

public class WorkoutTracking : BaseEntity
{
    public Guid WorkoutId { get; set; }
    public Workout Workout { get; set; }
    public DateTime StartWorkoutDate { get; set; }
    public DateTime EndWorkoutDate { get; set; }
    public string Note { get; set; }
    public ICollection<WorkoutSetTracking> Sets { get; set; }
}
