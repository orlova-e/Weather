using Weather.Domain.Entities;

namespace Weather.Infrastructure.Access;

public interface IEntityRepository<T>
    where T : class, IEntity
{
    Task<T> GetAsync(Guid id);
    Task<int> CountAsync();
    Task<List<T>> ListAsync();
    Task<List<T>> ListAsync(int page, int count, string sortField, bool asc = true);
    Task CreateAsync(T entity);
    Task CreateAsync(IEnumerable<T> entities);
}