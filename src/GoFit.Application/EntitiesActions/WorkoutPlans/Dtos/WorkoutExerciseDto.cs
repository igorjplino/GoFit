namespace GoFit.Application.EntitiesActions.WorkoutPlans.Dtos;

public record WorkoutExerciseDto
{
    public Guid ExerciseId { get; set; }
    public int Order { get; set; }
    public IEnumerable<WorkoutExerciseSetDto> Sets { get; set; } = new List<WorkoutExerciseSetDto>();
}
