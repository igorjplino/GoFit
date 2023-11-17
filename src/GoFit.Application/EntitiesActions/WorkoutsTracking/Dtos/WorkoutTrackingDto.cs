using GoFit.Application.EntitiesActions.Workouts.Dtos;

namespace GoFit.Application.EntitiesActions.WorkoutsTracking.Dtos;

public record WorkoutTrackingDto
{
    public Guid WorkoutId { get; set; }
    public WorkoutDto Workout { get; set; }
    public DateTime StartWorkoutDate { get; set; }
    public DateTime EndWorkoutDate { get; set; }
    public string? Note { get; set; }
    public ICollection<WorkoutSetTrackingDto> Sets { get; set; }
}
