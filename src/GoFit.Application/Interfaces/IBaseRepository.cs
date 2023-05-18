using GoFit.Domain.Common;

namespace GoFit.Application.Interfaces;
public interface IBaseRepository<T> where T : BaseEntity
{
    Guid Create(T entity);
    T Get(Guid id);
}
