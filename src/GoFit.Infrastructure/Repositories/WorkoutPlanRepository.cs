using GoFit.Application.Interfaces;
using GoFit.Domain.Entities;
using GoFit.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace GoFit.Infrastructure.Repositories;

public class WorkoutPlanRepository : BaseRepository<WorkoutPlan>, IWorkoutPlanRepository
{
    public WorkoutPlanRepository(GoFitDbContext context)
        : base(context)
    { }

    public async Task<WorkoutPlan?> GetPlanWithDetailsAsync(Guid id)
    {
        return await GetAsync(
            id: id,
            includes: source => 
                source
                    .Include(plan => plan.Workouts)
                        .ThenInclude(workout => workout.Sets)
                    .Include(plan => plan.Workouts)
                        .ThenInclude(workout => workout.Exercise));
    }
}
