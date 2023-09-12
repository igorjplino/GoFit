namespace GoFit.Application.EntitiesActions.WorkoutPlans.Dtos;

public record CreateWorkoutDto
{
    public string Name { get; set; }
    public IEnumerable<WorkoutExerciseDto> Exercises { get; set; }
}
