using GoFit.Domain.Common;

namespace GoFit.Domain.Entities;
public class Workout : BaseEntity
{
    public Guid WorkoutPlanId { get; set; }
    public WorkoutPlan WorkoutPlan { get; set; }
    public Guid ExerciseId { get; set; }
    public Exercise Exercise { get; set; }
    public int Order { get; set; }
    public ICollection<WorkoutSet> Sets { get; set; } = new List<WorkoutSet>();
}
