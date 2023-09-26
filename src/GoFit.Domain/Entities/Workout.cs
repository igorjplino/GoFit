using GoFit.Domain.Common;

namespace GoFit.Domain.Entities;
public class Workout : BaseEntity
{
    public string Name { get; set; }
    public string Description { get; set; }
    public int Order { get; set; }
    public Guid WorkoutPlanId { get; set; }
    public WorkoutPlan WorkoutPlan { get; set; }
    public ICollection<WorkoutExercise> WorkoutExercises { get; set; }
}
