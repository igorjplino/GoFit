namespace GoFit.Application.EntitiesActions.WorkoutPlans.Dtos;

public record WorkoutDto
{
    public string Name { get; set; }
    public string Description { get; set; }
    public int Order { get; set; }
    public IEnumerable<WorkoutExerciseDto> WorkoutExercises { get; set; }
}
