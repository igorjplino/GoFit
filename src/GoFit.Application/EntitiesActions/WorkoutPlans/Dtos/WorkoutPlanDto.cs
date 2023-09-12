namespace GoFit.Application.EntitiesActions.WorkoutPlans.Dtos;

public record WorkoutPlanDto
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public IEnumerable<WorkoutExerciseDto> Workouts { get; set; } = new List<WorkoutExerciseDto>();
}
