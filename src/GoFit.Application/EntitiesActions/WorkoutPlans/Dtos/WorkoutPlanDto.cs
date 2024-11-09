namespace GoFit.Application.EntitiesActions.WorkoutPlans.Dtos;

public record WorkoutPlanDto
{
    public Guid Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public IEnumerable<WorkoutDto> Workouts { get; set; }
}