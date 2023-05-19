using GoFit.Application.Interfaces;
using GoFit.Domain.Entities;

namespace GoFit.Infrastructure.Repositories;
public class WorkoutPlanRepository : BaseRepository<WorkoutPlan>, IWorkoutPlanRepository
{
    public override WorkoutPlan Get(Guid id)
    {
        return new WorkoutPlan { Title = "Test workout plan" };
    }
}
