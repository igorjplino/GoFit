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
            expression: x => x.Id == id,
            includes: source => 
                source
                    .Include(plan => plan.Workouts)
                        .ThenInclude(workout => workout.WorkoutExercises)
                            .ThenInclude(workoutSets => workoutSets.Sets)
                    .Include(plan => plan.Workouts)
                        .ThenInclude(workout => workout.WorkoutExercises)
                            .ThenInclude(workoutExercise => workoutExercise.Exercise));
    }

    public async Task<List<WorkoutPlan>> ListPlansByAthleteIdAsync(Guid athleteId)
    {
        return await ListAsync(
            expression: x => x.AthleteId == athleteId);
    }
}
