namespace GoFit.Application.EntitiesActions.WorkoutPlans.Dtos;

public class WorkoutExerciseSetDto
{
    public bool WarmUp { get; set; }
    public bool UntilFailure { get; set; }
    public int MinRepetitions { get; set; }
    public int MaxRepetitions { get; set; }
    public float ResetTime { get; set; }
    public float? Weight { get; set; }
    public int Order { get; set; }
}
