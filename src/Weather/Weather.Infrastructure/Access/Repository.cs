using Weather.Domain.Entities;
using Weather.Infrastructure.Mappings;

namespace Weather.Infrastructure.Access;

public class Repository : IRepository
{
    private readonly Context _context;
    private IEntityRepository<WeatherCondition> _weatherConditions;

    public Repository(Context context)
    {
        _context = context;
    }

    public IEntityRepository<WeatherCondition> WeatherConditions
    {
        get
        {
            if (_weatherConditions is null)
                _weatherConditions = new EntityRepository<WeatherCondition>(_context);
            return _weatherConditions;
        }
    }
}