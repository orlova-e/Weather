using MediatR;
using Weather.Web.Models.Common;
using Weather.Web.Models.Weather;

namespace Weather.Web.Services.Commands.Weather;

public class GetWeatherConditionsRequest : IRequest<CommandResult<WeatherListDto>>
{
    public GetEntitiesDto Dto { get; }

    public GetWeatherConditionsRequest(GetEntitiesDto dto)
    {
        Dto = dto;
    }
}