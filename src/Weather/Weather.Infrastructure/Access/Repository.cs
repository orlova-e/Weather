using Microsoft.EntityFrameworkCore;
using Weather.Domain.Entities;
using Weather.Infrastructure.Mappings;

namespace Weather.Infrastructure.Access;

public class Repository<T> : IEntityRepository<T>
    where T : class, IEntity
{
    private readonly Context _context;

    public Repository(Context context)
    {
        _context = context;
    }

    public async Task<T> GetAsync(Guid id)
    {
        return await _context.FindAsync<T>(id);
    }

    public Task<int> CountAsync()
    {
        return _context.Set<T>().CountAsync();
    }

    public Task<List<T>> ListAsync()
    {
        return _context.Set<T>().ToListAsync();
    }

    public Task<List<T>> ListAsync(
        int page,
        int count,
        string sortField,
        bool asc = true)
    {
        return _context
            .Set<T>()
            .Skip(page)
            .Take(count)
            .ToListAsync();
    }

    public Task CreateAsync(T entity)
    {
        _context.Add(entity);
        return _context.SaveChangesAsync();
    }

    public Task CreateAsync(IEnumerable<T> entities)
    {
        _context.AddRange(entities);
        return _context.SaveChangesAsync();
    }
}