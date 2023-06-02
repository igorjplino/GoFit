using GoFit.Application.Interfaces;
using GoFit.Domain.Entities;
using GoFit.Infrastructure.Contexts;

namespace GoFit.Infrastructure.Repositories;
public class WorkoutPlanRepository : BaseRepository<WorkoutPlan>, IWorkoutPlanRepository
{
    public WorkoutPlanRepository(GoFitDbContext context)
        : base(context)
    { }
}
