using GoFit.Application.Interfaces;
using GoFit.Domain.Common;

namespace GoFit.Infrastructure.Repositories;
public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
{
    public Guid Create(T entity)
    {
        throw new NotImplementedException();
    }

    public virtual T Get(Guid id)
    {
        throw new NotImplementedException();
    }
}
