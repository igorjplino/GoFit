using GoFit.Application.Interfaces;
using GoFit.Domain.Entities;
using GoFit.Infrastructure.Contexts;
using GoFit.Infrastructure.Contexts.GoFitDb;

namespace GoFit.Infrastructure.Repositories;
public class AthleteRepository : BaseRepository<Athlete>, IAthleteRepository
{
    public AthleteRepository(GoFitDbContext context)
        : base(context)
    { }

    public async Task<Athlete?> GetByAppUserIdAsync(string appUserId)
    {
        return await GetAsync(expression: x => x.AppUserId == appUserId);
    }
}
