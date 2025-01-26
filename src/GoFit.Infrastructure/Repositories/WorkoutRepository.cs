using GoFit.Application.Interfaces;
using GoFit.Domain.Entities;
using GoFit.Infrastructure.Contexts;
using GoFit.Infrastructure.Contexts.GoFitDb;

namespace GoFit.Infrastructure.Repositories;
public class WorkoutRepository : BaseRepository<Workout>, IWorkoutRepository
{
    public WorkoutRepository(GoFitDbContext context)
        : base(context)
    { }
}