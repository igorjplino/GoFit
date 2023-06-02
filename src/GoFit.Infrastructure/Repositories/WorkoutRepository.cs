using GoFit.Application.Interfaces;
using GoFit.Domain.Entities;
using GoFit.Infrastructure.Contexts;

namespace GoFit.Infrastructure.Repositories;
public class WorkoutRepository : BaseRepository<Workout>, IWorkoutRepository
{
    public WorkoutRepository(GoFitDbContext context)
        : base(context)
    { }
}