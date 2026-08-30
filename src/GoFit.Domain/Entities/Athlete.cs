namespace GoFit.Domain.Entities;
public class Athlete : Person
{
    public string? AppUserId { get; set; }
    public ICollection<WorkoutPlan> WorkoutPlans { get; set; }
}
