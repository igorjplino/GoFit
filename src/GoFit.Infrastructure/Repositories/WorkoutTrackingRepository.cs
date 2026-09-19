using GoFit.Application.Interfaces;
using GoFit.Domain.Entities;
using GoFit.Infrastructure.Contexts;
using GoFit.Infrastructure.Contexts.GoFitDb;
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
            expression: x => x.Id == id,
            includes: source =>
                source
                    .Include(o => o.Sets)
                    .Include(o => o.Workout));
    }

    public async Task<WorkoutTracking?> GetActiveByAthleteIdAsync(Guid athleteId)
    {
        return await GetAsync(
            expression: x => x.AthleteId == athleteId && x.EndWorkoutDate == null && x.CancelledDate == null,
            includes: source =>
                source
                    .Include(o => o.Sets)
                    .Include(o => o.Workout));
    }

    public async Task<List<WorkoutTracking>> ListHistoryByAthleteIdAsync(Guid athleteId)
    {
        return await ListAsync(
            expression: x => x.AthleteId == athleteId && (x.EndWorkoutDate != null || x.CancelledDate != null),
            includes: source =>
                source
                    .Include(o => o.Sets)
                    .Include(o => o.Workout),
            orderBy: source => source.OrderByDescending(x => x.StartWorkoutDate));
    }

    public async Task CancelWorkoutTrackingAsync(Guid id, DateTime cancelledDate)
    {
        await Context.WorkoutsTracking
            .Where(x => x.Id == id)
            .ExecuteUpdateAsync(wt => wt.SetProperty(x => x.CancelledDate, cancelledDate));
    }

    public async Task FinishWorkoutTrackingAsync(Guid id, DateTime endWorkoutDate)
    {
        await Context.WorkoutsTracking
            .Where(x => x.Id == id)
            .ExecuteUpdateAsync(wt => wt.SetProperty(x => x.EndWorkoutDate, endWorkoutDate));
    }
}
