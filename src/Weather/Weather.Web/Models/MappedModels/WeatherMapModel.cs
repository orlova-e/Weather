using Weather.Services.MappingModels;

namespace Weather.Services.MappedModels;

public class WeatherMapModel : IMapModel
{
    [ColumnMap(1, "Дата")]
    public DateTime Date { get; set; }
    [ColumnMap(2, "Время")]
    public DateTime Time { get; set; }
    [ColumnMap(3, "Т")]
    public decimal Temperature { get; set; }
    [ColumnMap(4, "Отн. влажность")]
    public decimal RelativeHumidity { get; set; }
    [ColumnMap(5, "Td")]
    public decimal DewPoint { get; set; }
    [ColumnMap(6, "Атм. давление")]
    public int AtmosphericPressure { get; set; }
    [ColumnMap(7, "Направление")]
    public string WindDirection { get; set; }
    [ColumnMap(8, "Скорость")]
    public int WindSpeed { get; set; }
    [ColumnMap(9, "Облачность")]
    public decimal CloudCover { get; set; }
    [ColumnMap(10, "h")]
    public int LowerCloudLimit { get; set; }
    [ColumnMap(11, "VV")]
    public string HorizontalVisibility { get; set; }
    [ColumnMap(12, "Погодные явления")]
    public string Phenomena { get; set; }
}