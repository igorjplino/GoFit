using GoFit.Application.Interfaces;
using GoFit.Domain.Entities;

namespace GoFit.Infrastructure.Repositories;
public class WorkoutRepository : BaseRepository<Workout>, IWorkoutRepository
{
    public override Workout Get(Guid id)
    {
        return new Workout { Name = "Test", Description = "Testing.." };
    }
}