using GoFit.Application.EntitiesActions.Exercises.Queries;
using GoFit.Application.Models;
using GoFit.Domain.Entities;

namespace GoFit.Application.Interfaces;
public interface IExerciseRepository : IBaseRepository<Exercise>
{
    Task<Exercise?> GetByNameAsync(string name);
    Task<(IEnumerable<Exercise> Exercises, int Total)> ListExercisesAsync(GetAllExercisesQuery filters);
}
