using GoFit.Domain.Common;

namespace GoFit.Domain.Entities;
public class Workout : BaseEntity
{
    public Guid ExerciseId { get; set; }
    public int Order { get; set; }
    public IEnumerable<WorkoutSet>? Sets { get; set; }
}
