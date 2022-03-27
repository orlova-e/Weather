using Weather.Domain.Enums;

namespace Weather.Domain.Entities;

public class WeatherCondition : IEntity
{
    public Guid Id { get; set; }
    public DateTime DateTime { get; set; }
    public decimal Temperature { get; set; }
    public decimal RelativeHumidity { get; set; }
    public decimal DewPoint { get; set; }
    public int AtmosphericPressure { get; set; }
    public WindDirection WindDirection { get; set; }
    public int WindSpeed { get; set; }
    public decimal CloudCover { get; set; }
    public int LowerCloudLimit { get; set; }
    public int? HorizontalVisibility { get; set; }
    public string Phenomena { get; set; }
}