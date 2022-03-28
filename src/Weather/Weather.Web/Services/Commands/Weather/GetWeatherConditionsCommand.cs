using System.Linq.Expressions;
using AutoMapper;
using MediatR;
using Weather.Domain.Entities;
using Weather.Infrastructure.Access;
using Weather.Infrastructure.Helpers;
using Weather.Services.Data;
using Weather.Web.Models.Weather;

namespace Weather.Web.Services.Commands.Weather;

public class GetWeatherConditionsCommand : IRequestHandler<GetWeatherConditionsRequest, CommandResult<WeatherListDto>>
{
    private readonly IRepository _repository;
    private readonly IInfoService _infoService;
    private readonly IMapper _mapper;
    private readonly ILogger<GetWeatherConditionsCommand> _logger;

    public GetWeatherConditionsCommand(
        IRepository repository,
        IInfoService infoService,
        IMapper mapper,
        ILogger<GetWeatherConditionsCommand> logger)
    {
        _repository = repository;
        _infoService = infoService;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<CommandResult<WeatherListDto>> Handle(
        GetWeatherConditionsRequest request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Get weather conditions command is started");

        try
        {
            Expression<Func<WeatherCondition, bool>> expression = null;
            if (FilterParser.TryParseFilter(request.Dto.Filters, out Dictionary<string, string> filters))
            {
                for (int i = 0; i < filters.Count; i++)
                {
                    var right = ExpressionHelper.BuildExpression<WeatherCondition>(
                        filters.ElementAt(i).Key,
                        filters.ElementAt(i).Value);
                    
                    if (i == 0)
                    {
                        expression = right;
                        continue;
                    }

                    expression = expression.And(right);
                }
            }
            
            
            var entities = await _repository.WeatherConditions.List(
                wherePredicate: expression,
                orderByKey: request.Dto.OrderBy ?? nameof(WeatherCondition.DateTime),
                sortDir: request.Dto.SortDir,
                skip: request.Dto.Page * request.Dto.ItemsNumber,
                take: request.Dto.ItemsNumber);
            
            var totalEntitiesCount = await _repository.WeatherConditions.CountAsync(expression);
            var viewModels = _mapper.Map<IEnumerable<WeatherCondition>, IEnumerable<WeatherViewDto>>(entities);
            
            var listDto = new WeatherListDto()
            {
                CurrentPage = request.Dto.Page,
                ItemsPerPage = request.Dto.ItemsNumber,
                ListSorting = request.Dto.SortDir,
                TotalPages = (int) Math.Ceiling((double) totalEntitiesCount / request.Dto.ItemsNumber),
                TotalItems = totalEntitiesCount,
                Entities = viewModels,
                Filters = request.Dto.Filters,
                AvailableMonths = await _infoService.GetAvailableMonths(),
                AvailableYears = await _infoService.GetAvailableYears()
            };
            
            _logger.LogInformation("{count} of weather conditions entities were recieved", entities.Count);
            return new CommandResult<WeatherListDto>(listDto);
        }
        catch (Exception exc)
        {
            _logger.LogError("Couldn't receive entities of type '{type}':\n{message}\n{stackTrace}",
                typeof(WeatherCondition), exc.Message, exc.StackTrace);
            
            return new CommandResult<WeatherListDto>(isError: true);
        }
    }
}