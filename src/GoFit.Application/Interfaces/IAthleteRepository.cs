using GoFit.Domain.Entities;

namespace GoFit.Application.Interfaces;
public interface IAthleteRepository : IBaseRepository<Athlete>
{
    Task<Athlete?> GetByAppUserIdAsync(string appUserId);
}
