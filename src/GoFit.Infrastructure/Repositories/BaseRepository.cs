using GoFit.Application.Interfaces;
using GoFit.Domain.Common;
using GoFit.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

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

    public async Task UpdateAsync(T entity)
    {
        _context.Set<T>().Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _context.Set<T>().ToListAsync();
    }

    public async Task<T?> GetAsync(Guid id)
    {
        return await _context.FindAsync<T>(id);
    }

    protected async Task<T?> GetAsync(Expression<Func<T, bool>> expression, Func<IQueryable<T>, IIncludableQueryable<T, object>>? includes = null)
    {
        IQueryable<T> query = Context.Set<T>();

        if (includes is not null)
        {
            query = includes(query);
        }

        return await query.FirstOrDefaultAsync(expression);
    }

    protected async Task<List<T>> ListAsync(Expression<Func<T, bool>> expression, Func<IQueryable<T>, IIncludableQueryable<T, object>>? includes = null)
    {
        IQueryable<T> query = Context.Set<T>();

        if (includes is not null)
        {
            query = includes(query);
        }

        return await query.Where(expression).ToListAsync();
    }
}
