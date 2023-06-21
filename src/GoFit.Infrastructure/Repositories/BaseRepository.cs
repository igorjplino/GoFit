using GoFit.Application.Interfaces;
using GoFit.Domain.Common;
using GoFit.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace GoFit.Infrastructure.Repositories;
public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
{
    protected readonly GoFitDbContext _context;

    public BaseRepository(GoFitDbContext context)
    {
        _context = context;
    }

    protected GoFitDbContext Context => _context;

    public async Task<Guid> CreateAsync(T entity)
    {
        await _context.AddAsync(entity);
        await _context.SaveChangesAsync();

        return entity.Id;
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _context.Set<T>().ToListAsync();
    }

    public virtual async Task<T?> GetAsync(Guid id)
    {
        return await _context.FindAsync<T>(id);
    }
}
