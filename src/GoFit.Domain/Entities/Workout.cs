using GoFit.Domain.Common;

namespace GoFit.Domain.Entities;
public class Workout : BaseEntity
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public IEnumerable<WorkoutSet>? Sets { get; set; }
    public IEnumerable<WorkoutSet>? HistorySets { get; set; }
}
