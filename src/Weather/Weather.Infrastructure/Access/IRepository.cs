using Weather.Domain.Entities;

namespace Weather.Infrastructure.Access;

public interface IRepository
{
    IEntityRepository<WeatherCondition> WeatherConditions { get; }
}