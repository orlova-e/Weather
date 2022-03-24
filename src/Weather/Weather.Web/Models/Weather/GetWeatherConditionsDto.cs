using Weather.Web.Models.Common;

namespace Weather.Web.Models.Weather;

public class GetWeatherConditionsDto : GetEntitiesDto
{
    public WeatherViewDto Dto { get; set; }
}