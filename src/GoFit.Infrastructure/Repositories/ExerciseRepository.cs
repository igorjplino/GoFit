using GoFit.Application.Interfaces;
using GoFit.Domain.Entities;
using GoFit.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace GoFit.Infrastructure.Repositories;
public class ExerciseRepository : BaseRepository<Exercise>, IExerciseRepository
{
    public ExerciseRepository(GoFitDbContext context) : base(context)
    { }

    public async Task<Exercise?> GetByNameAsync(string name)
    {
        return await Context.Set<Exercise>().FirstOrDefaultAsync(x => x.Name == name);
    }
}
