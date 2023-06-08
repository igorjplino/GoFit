using GoFit.Domain.Common;

namespace GoFit.Application.Interfaces;
public interface IBaseRepository<T> where T : BaseEntity
{
    Task<Guid> CreateAsync(T entity);
    Task<T?> GetAsync(Guid id);
}
