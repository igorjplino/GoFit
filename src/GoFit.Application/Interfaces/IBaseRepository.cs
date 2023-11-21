using GoFit.Domain.Common;

namespace GoFit.Application.Interfaces;
public interface IBaseRepository<T> where T : BaseEntity
{
    Task<Guid> CreateAsync(T entity);
    Task UpdateAsync(T entity);
    Task<IEnumerable<T>> GetAllAsync();
    Task<T?> GetAsync(Guid id);
}
