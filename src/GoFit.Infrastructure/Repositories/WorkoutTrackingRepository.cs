using GoFit.Application.Interfaces;
using GoFit.Domain.Entities;
using GoFit.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace GoFit.Infrastructure.Repositories;

public class WorkoutTrackingRepository : BaseRepository<WorkoutTracking>, IWorkoutTrackingRepository
{
    public WorkoutTrackingRepository(GoFitDbContext context)
        : base(context)
    { }

    public async Task<WorkoutTracking?> GetWithSetsAsync(Guid id)
    {
        return await GetAsync(
            id: id,
            includes: source =>
                source
                    .Include(o => o.Sets)
                    .Include(o => o.Workout));
    }
}
