using GoFit.Application.Interfaces;
using GoFit.Domain.Entities;
using GoFit.Infrastructure.Contexts;
using GoFit.Infrastructure.Contexts.GoFitDb;
using Microsoft.EntityFrameworkCore;

namespace GoFit.Infrastructure.Repositories;
public class WorkoutRepository : BaseRepository<Workout>, IWorkoutRepository
{
    public WorkoutRepository(GoFitDbContext context)
        : base(context)
    { }

    public async Task<Workout?> GetWithDetailsAsync(Guid id)
    {
        return await GetAsync(
            expression: x => x.Id == id,
            includes: source =>
                source
                    .Include(w => w.WorkoutExercises)
                        .ThenInclude(we => we.Sets)
                    .Include(w => w.WorkoutExercises)
                        .ThenInclude(we => we.Exercise)
                    .Include(w => w.WorkoutPlan));
    }

    public async Task UpdateWorkoutAsync(Guid workoutId, string name, string? description, IEnumerable<WorkoutExercise> exercises)
    {
        using var transaction = Context.Database.BeginTransaction();

        try
        {
            await Context.WorkoutsExercises
                .Where(x => x.WorkoutId == workoutId)
                .ExecuteDeleteAsync();

            await Context.Workouts
                .Where(x => x.Id == workoutId)
                .ExecuteUpdateAsync(w => w
                    .SetProperty(x => x.Name, name)
                    .SetProperty(x => x.Description, description));

            await Context.WorkoutsExercises.AddRangeAsync(exercises);

            await Context.SaveChangesAsync();

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
