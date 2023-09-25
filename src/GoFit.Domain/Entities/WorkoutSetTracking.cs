using GoFit.Domain.Common;

namespace GoFit.Domain.Entities;
public class WorkoutSetTracking : BaseEntity
{
    public Guid WorkoutTrackingId { get; set; }
    public WorkoutTracking WorkoutTracking { get; set; }
    public int Repetitions { get; set; }
    public float Weight { get; set; }
    public int Order { get; set; }
}
