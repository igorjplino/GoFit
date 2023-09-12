using GoFit.Domain.Common;

namespace GoFit.Domain.Entities;
public class WorkoutSet : BaseEntity
{
    public Guid WorkoutId { get; set; }
    public Workout Workout { get; set; }
    public bool WarmUp { get; set; }
    public bool UntilFailure { get; set; }
    public int MinRepetitions { get; set; }
    public int MaxRepetitions { get; set; }
    public float ResetTime { get; set; }
    public float? Weight { get; set; }
    public int Order { get; set; }
}
