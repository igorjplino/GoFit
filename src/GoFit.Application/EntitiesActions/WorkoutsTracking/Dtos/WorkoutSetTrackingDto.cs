namespace GoFit.Application.EntitiesActions.WorkoutsTracking.Dtos;

public record WorkoutSetTrackingDto
{
    public int Repetitions { get; set; }
    public float Weight { get; set; }
    public int Order { get; set; }
}
