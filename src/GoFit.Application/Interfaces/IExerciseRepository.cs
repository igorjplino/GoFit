using GoFit.Domain.Entities;

namespace GoFit.Application.Interfaces;
public interface IExerciseRepository : IBaseRepository<Exercise>
{
    Task<Exercise?> GetByNameAsync(string name);
}
