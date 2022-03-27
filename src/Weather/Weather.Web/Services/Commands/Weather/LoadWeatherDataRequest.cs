using MediatR;
using Weather.Web.Models.Weather;

namespace Weather.Web.Services.Commands.Weather;

public class LoadWeatherDataRequest : IRequest<CommandResult<Unit>>
{
    public LoadWeatherDto Dto { get; }

    public LoadWeatherDataRequest(LoadWeatherDto dto)
    {
        Dto = dto;
    }
}