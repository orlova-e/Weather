using Weather.Web.Models.Common;

namespace Weather.Web.Models.Weather;

public class WeatherListDto : ListDto<WeatherViewDto>
{
    public IEnumerable<int> AvailableMonths { get; set; }
    public IEnumerable<int> AvailableYears { get; set; }
}