using GoFit.Application.Interfaces;
using GoFit.Domain.Common;
using GoFit.Infrastructure.Contexts;

namespace GoFit.Infrastructure.Repositories;
public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
{
    protected readonly GoFitDbContext _context;

    public BaseRepository(GoFitDbContext context)
    {
        _context = context;
    }

    public Guid Create(T entity)
    {
        _context.Add(entity);
        _context.SaveChanges();

        return entity.Id;
    }

    public virtual T? Get(Guid id)
    {
        return _context.Find<T>(id);
    }
}
