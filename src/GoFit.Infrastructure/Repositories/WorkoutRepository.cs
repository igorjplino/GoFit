using GoFit.Application.Interfaces;
using GoFit.Domain.Entities;

namespace GoFit.Infrastructure.Repositories;
public class WorkoutRepository : BaseRepository<Exercise>, IWorkoutRepository
{
    public override Exercise Get(Guid id)
    {
        return new Exercise { Name = "Test", Description = "Testing.." };
    }
}