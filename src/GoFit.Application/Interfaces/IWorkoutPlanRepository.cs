using GoFit.Domain.Entities;

namespace GoFit.Application.Interfaces;

public interface IWorkoutPlanRepository : IBaseRepository<WorkoutPlan>
{
    Task<WorkoutPlan?> GetPlanWithDetailsAsync(Guid id);
    Task<List<WorkoutPlan>> ListPlansByAthleteIdAsync(Guid athleteId);
    Task UpdateTitleAndDescriptionAsync(Guid id, string title, string? description);
}
