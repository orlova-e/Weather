using Weather.Infrastructure.Access;

namespace Weather.Services.Data;

public class InfoService : IInfoService
{
    private readonly IRepository _repository;

    public InfoService(IRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<int>> GetAvailableYears()
    {
        var entities = await _repository.WeatherConditions.ListAsync();
        return entities?
            .Distinct()
            .Select(x => x.DateTime.Year);
    }
    
    public async Task<IEnumerable<int>> GetAvailableMonths()
    {
        var entities = await _repository.WeatherConditions.ListAsync();
        return entities?
            .Distinct()
            .Select(x => x.DateTime.Month);
    }
}