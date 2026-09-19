using GoFit.Application.Interfaces;
using GoFit.Application.Models;
using GoFit.Domain.Entities;
using GoFit.Infrastructure.Contexts.GoFitDb;
using Microsoft.EntityFrameworkCore;

namespace GoFit.Infrastructure.Repositories;

public class WorkoutSetTrackingRepository : BaseRepository<WorkoutSetTracking>, IWorkoutSetTrackingRepository
{
    public WorkoutSetTrackingRepository(GoFitDbContext context)
        : base(context)
    { }

    public async Task<List<WorkoutSetTracking>> ListByTrackingIdAsync(Guid workoutTrackingId)
    {
        return await ListAsync(
            expression: x => x.WorkoutTrackingId == workoutTrackingId,
            orderBy: source => source.OrderBy(x => x.Order));
    }

    public async Task UpdateSetAsync(Guid setId, int repetitions, float weight)
    {
        await Context.WorkoutSetsTracking
            .Where(x => x.Id == setId)
            .ExecuteUpdateAsync(set => set
                .SetProperty(x => x.Repetitions, repetitions)
                .SetProperty(x => x.Weight, weight));
    }

    public async Task RemoveAndResequenceAsync(Guid setId, IReadOnlyCollection<WorkoutSetPosition> resequencedSets)
    {
        // One transaction: a removal that renumbered only some of the remaining sets would leave duplicate
        // positions behind, and the n-th logged set would no longer pair with the n-th planned set.
        await using var transaction = await Context.Database.BeginTransactionAsync();

        try
        {
            await Context.WorkoutSetsTracking
                .Where(x => x.Id == setId)
                .ExecuteDeleteAsync();

            foreach (var position in resequencedSets)
            {
                await Context.WorkoutSetsTracking
                    .Where(x => x.Id == position.SetId)
                    .ExecuteUpdateAsync(set => set.SetProperty(x => x.Order, position.Order));
            }

            await transaction.CommitAsync();
        }
        catch (Exception)
        {
            //TODO: handle and log error
            await transaction.RollbackAsync();
            throw;
        }
    }
}
