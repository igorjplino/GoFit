using GoFit.Domain.Common;

namespace GoFit.Domain.Entities;
public class WorkoutSet : BaseEntity
{
    public float Weight { get; set; }
    public int Repetitions { get; set; }
}
