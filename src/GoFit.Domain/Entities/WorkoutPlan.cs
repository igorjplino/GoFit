using GoFit.Domain.Common;

namespace GoFit.Domain.Entities;

public class WorkoutPlan : BaseEntity
{
    public WorkoutPlan()
    {
        Workouts = new List<Workout>();
    }

    public Guid AthleteId { get; set; }
    public Athlete Athlete { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public ICollection<Workout> Workouts { get; set; }
}
