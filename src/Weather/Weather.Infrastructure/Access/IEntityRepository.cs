using System.Linq.Expressions;
using Weather.Domain.Entities;
using Weather.Infrastructure.Conditions;

namespace Weather.Infrastructure.Access;

public interface IEntityRepository<T>
    where T : class, IEntity
{
    Task<int> CountAsync();
    Task<int> CountAsync(Expression<Func<T, bool>> wherePredicate);
    Task<List<T>> ListAsync();
    Task<List<T>> List(string orderByKey, SortDir sortDir = SortDir.Asc, 
        Expression<Func<T, bool>> wherePredicate = null, int skip = 1, int take = 20);
    Task CreateAsync(T entity);
    Task CreateAsync(IEnumerable<T> entities);
}