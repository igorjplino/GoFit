using GoFit.Application.Interfaces;
using GoFit.Domain.Entities;
using GoFit.Infrastructure.Contexts;

namespace GoFit.Infrastructure.Repositories;
public class ExerciseRepository : BaseRepository<Exercise>, IExerciseRepository
{
    public ExerciseRepository(GoFitDbContext context) : base(context)
    { }
}
