namespace GoFit.Application.EntitiesActions.WorkoutPlans.Dtos;

public record WorkoutExerciseDto
{
    public Guid ExerciseId { get; set; }
    public string? ExerciseName { get; set; }
    public string? ExerciseDescription { get; set; }
    public int Order { get; set; }
    public IEnumerable<WorkoutExerciseSetDto> Sets { get; set; } = new List<WorkoutExerciseSetDto>();
}
