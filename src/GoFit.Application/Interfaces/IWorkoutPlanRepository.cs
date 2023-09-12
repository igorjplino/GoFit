using GoFit.Domain.Entities;

namespace GoFit.Application.Interfaces;

public interface IWorkoutPlanRepository : IBaseRepository<WorkoutPlan>
{
    Task<WorkoutPlan?> GetPlanWithDetailsAsync(Guid id);
}
