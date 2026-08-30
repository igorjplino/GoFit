using GoFit.Application.EntitiesActions.WorkoutPlans.Dtos;

namespace GoFit.Application.EntitiesActions.Workouts.Dtos;
public record WorkoutDto
{
    public Guid Id { get; set; }
    public Guid WorkoutPlanId { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public int Order { get; set; }
    public IEnumerable<WorkoutExerciseDto> WorkoutExercises { get; set; } = Enumerable.Empty<WorkoutExerciseDto>();
}
