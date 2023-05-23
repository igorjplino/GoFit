using GoFit.Domain.Common;

namespace GoFit.Domain.Entities;
public class WorkoutPlan : BaseEntity
{
    public string Title { get; set; }
    public string? Description { get; set; }
    public IEnumerable<Guid>? WorkoutsId { get; set; }
}
