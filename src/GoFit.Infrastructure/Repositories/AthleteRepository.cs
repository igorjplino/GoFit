using GoFit.Application.Interfaces;
using GoFit.Domain.Entities;
using GoFit.Infrastructure.Contexts;

namespace GoFit.Infrastructure.Repositories;
public class AthleteRepository : BaseRepository<Athlete>, IAthleteRepository
{
    public AthleteRepository(GoFitDbContext context)
        : base(context)
    { }
}
