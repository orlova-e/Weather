namespace Weather.Domain.Entities;

public class City : IEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public IEnumerable<WeatherCondition> WeatherConditions { get; set; }
}