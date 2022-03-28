using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Weather.Domain.Entities;
using Weather.Infrastructure.Conditions;
using Weather.Infrastructure.Mappings;
using Weather.Infrastructure.Helpers;

namespace Weather.Infrastructure.Access;

public class EntityRepository<T> : IEntityRepository<T>
    where T : class, IEntity
{
    private readonly Context _context;

    public EntityRepository(Context context)
    {
        _context = context;
    }

    public Task<int> CountAsync()
    {
        return _context.Set<T>().CountAsync();
    }

    public Task<int> CountAsync(Expression<Func<T, bool>> wherePredicate)
    {
        wherePredicate ??= x => true;
        
        return _context
            .Set<T>()
            .Where(wherePredicate)
            .CountAsync();
    }

    public async Task<List<T>> ListAsync()
    {
        return await _context
            .Set<T>()
            .Where(x => true)
            .ToListAsync();
    }
    
    public Task<List<T>> List(
        string orderByKey,
        SortDir sortDir = SortDir.Asc,
        Expression<Func<T, bool>> wherePredicate = null,
        int skip = 1,
        int take = 20)
    {
        wherePredicate ??= x => true;
        
        return _context
            .Set<T>()
            .Where(wherePredicate)
            .OrderBy<T>(orderByKey, sortDir)
            .Skip(skip)
            .Take(take)
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