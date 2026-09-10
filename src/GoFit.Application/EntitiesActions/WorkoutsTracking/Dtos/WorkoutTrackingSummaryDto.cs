namespace GoFit.Application.EntitiesActions.WorkoutsTracking.Dtos;

/// <summary>
/// A past workout as it appears in a history list. Deliberately flat: the full
/// <see cref="WorkoutTrackingDto"/> carries the nested workout tree and every logged set,
/// none of which a summary row needs.
/// </summary>
public record WorkoutTrackingSummaryDto
{
    public Guid Id { get; set; }
    public Guid WorkoutId { get; set; }
    public string? WorkoutName { get; set; }
    public DateTime StartWorkoutDate { get; set; }
    public DateTime? EndWorkoutDate { get; set; }
    public DateTime? CancelledDate { get; set; }
    public string? Note { get; set; }
    public int SetCount { get; set; }
}
