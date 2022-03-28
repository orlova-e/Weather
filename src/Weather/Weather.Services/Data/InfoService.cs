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
            .OrderByDescending(x => x.DateTime.Year)
            .Select(x => x.DateTime.Year)
            .Distinct();
    }
    
    public async Task<IEnumerable<int>> GetAvailableMonths()
    {
        var entities = await _repository.WeatherConditions.ListAsync();
        return entities?
            .OrderByDescending(x => x.DateTime.Month)
            .Select(x => x.DateTime.Month)
            .Distinct();
    }
}