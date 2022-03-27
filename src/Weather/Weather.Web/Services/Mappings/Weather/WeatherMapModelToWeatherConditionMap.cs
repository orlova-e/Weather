using AutoMapper;
using Weather.Domain.Entities;
using Weather.Domain.Enums;
using Weather.Services.MappedModels;

namespace Weather.Web.Services.Mappings.Weather;

public class WeatherMapModelToWeatherConditionMap : Profile
{
    public WeatherMapModelToWeatherConditionMap()
    {
        CreateMap<WeatherMapModel, WeatherCondition>()
            .ForMember(x => x.Id, o => o.Ignore())
            .ForMember(x => x.DateTime, o => o.MapFrom(x => x.Date.AddTicks(x.Time.Ticks)))
            .ForMember(x => x.Temperature, o => o.MapFrom(x => x.Temperature))
            .ForMember(x => x.RelativeHumidity, o => o.MapFrom(x => x.RelativeHumidity))
            .ForMember(x => x.DewPoint, o => o.MapFrom(x => x.DewPoint))
            .ForMember(x => x.AtmosphericPressure, o => o.MapFrom(x => x.AtmosphericPressure))
            .ForMember(x => x.WindDirection, o => o.MapFrom(x=> GetWindDirection(x.WindDirection)))
            .ForMember(x => x.CloudCover, o => o.MapFrom(x => x.CloudCover))
            .ForMember(x => x.LowerCloudLimit, o => o.MapFrom(x => x.LowerCloudLimit))
            .ForMember(x => x.HorizontalVisibility, o => o.MapFrom(x => x.HorizontalVisibility))
            .ForMember(x => x.Phenomena, o => o.MapFrom(x => x.Phenomena));
    }

    private WindDirection GetWindDirection(string windDirection)
        => windDirection switch
        {
            WindDirections.Northern => WindDirection.Northern,
            WindDirections.Northwest => WindDirection.Northwest,
            WindDirections.Western => WindDirection.Western,
            WindDirections.Southwest => WindDirection.Southwest,
            WindDirections.Southern => WindDirection.Southern,
            WindDirections.SouthEast => WindDirection.SouthEast,
            WindDirections.Eastern => WindDirection.Eastern,
            WindDirections.Northeast => WindDirection.Northeast,
            WindDirections.Calm => WindDirection.Calm,
            _ => throw new ArgumentException(nameof(windDirection)),
        };
}