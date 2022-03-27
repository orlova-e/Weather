using AutoMapper;
using MediatR;
using Weather.Domain.Entities;
using Weather.Infrastructure.Access;
using Weather.Infrastructure.Conditions;
using Weather.Web.Models.Common;
using Weather.Web.Models.Weather;

namespace Weather.Web.Services.Commands.Weather;

public class GetWeatherConditionsCommand : IRequestHandler<GetWeatherConditionsRequest, CommandResult<ListDto<WeatherViewDto>>>
{
    private readonly IRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetWeatherConditionsCommand> _logger;

    public GetWeatherConditionsCommand(
        IRepository repository,
        IMapper mapper,
        ILogger<GetWeatherConditionsCommand> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<CommandResult<ListDto<WeatherViewDto>>> Handle(
        GetWeatherConditionsRequest conditionsRequest,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Get weather conditions command is started");

        try
        {
            var entities = await _repository.WeatherConditions.ListAsync(
                page: conditionsRequest.Dto.Page,
                count: conditionsRequest.Dto.ItemsNumber,
                sortField: nameof(WeatherCondition.DateTime),
                asc: conditionsRequest.Dto.SortDir == SortDir.Asc);
            
            var totalEntitiesCount = await _repository.WeatherConditions.CountAsync();

            var viewModels = _mapper.Map<IEnumerable<WeatherCondition>, IEnumerable<WeatherViewDto>>(entities);

            var listDto = new ListDto<WeatherViewDto>()
            {
                CurrentPage = conditionsRequest.Dto.Page,
                ItemsPerPage = entities.Count,
                ListSorting = conditionsRequest.Dto.SortDir,
                TotalPages = (int) Math.Round(
                    totalEntitiesCount / (double) conditionsRequest.Dto.ItemsNumber, 
                    MidpointRounding.ToPositiveInfinity),
                TotalItems = totalEntitiesCount,
                Entities = viewModels
            };
            
            _logger.LogInformation("{count} of weather conditions entities were recieved", entities.Count);
            return new CommandResult<ListDto<WeatherViewDto>>(listDto);
        }
        catch (Exception exc)
        {
            _logger.LogError("Couldn't receive entities of type '{type}':\n{message}\n{stackTrace}",
                typeof(WeatherCondition), exc.Message, exc.StackTrace);
            
            return new CommandResult<ListDto<WeatherViewDto>>(isError: true);
        }
    }
}