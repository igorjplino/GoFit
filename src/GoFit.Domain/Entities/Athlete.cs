namespace GoFit.Domain.Entities;
public class Athlete : Person
{
    public ICollection<WorkoutPlan> WorkoutPlans { get; set; }
}
