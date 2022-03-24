namespace Weather.Web.Models.Weather;

public class LoadWeatherDto
{
    public IEnumerable<IFormFile> Files { get; set; }
}