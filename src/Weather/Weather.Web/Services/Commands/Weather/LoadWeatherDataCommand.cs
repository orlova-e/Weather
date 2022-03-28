using AutoMapper;
using MediatR;
using Microsoft.Extensions.Options;
using Weather.Domain.Entities;
using Weather.Infrastructure.Access;
using Weather.Services.Files;
using Weather.Services.MappedModels;
using Weather.Web.Models.Configuration;

namespace Weather.Web.Services.Commands.Weather;

public class LoadWeatherDataCommand : IRequestHandler<LoadWeatherDataRequest, CommandResult<Unit>>
{
    private readonly IRepository _repository;
    private readonly IFileService<WeatherMapModel> _fileService;
    private readonly IOptions<FileSettings> _options;
    private readonly IMapper _mapper;
    private readonly ILogger<LoadWeatherDataCommand> _logger;

    public LoadWeatherDataCommand(
        IRepository repository,
        IFileService<WeatherMapModel> fileService,
        IOptions<FileSettings> options,
        IMapper mapper,
        ILogger<LoadWeatherDataCommand> logger)
    {
        _repository = repository;
        _fileService = fileService;
        _options = options;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<CommandResult<Unit>> Handle(
        LoadWeatherDataRequest request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Start loading weather conditions");

        try
        {
            var mapModels = new List<WeatherMapModel>();
            
            foreach (var file in request.Dto.Files)
            {
                await using var stream = file.OpenReadStream();
                _fileService.TryReadFile(stream, out IList<WeatherMapModel> mappedModels, _options.Value.SkipRows);
                mapModels.AddRange(mappedModels);
            }

            var conditions = _mapper
                .Map<IEnumerable<WeatherMapModel>, IEnumerable<WeatherCondition>>(mapModels)?
                .ToList();

            await _repository.WeatherConditions.CreateAsync(conditions);
        }
        catch (Exception exc)
        {
            _logger.LogError("Unable to parse file(s):\n{message}\n{stackTrace}",
                exc.Message, exc.StackTrace);
            
            return new CommandResult<Unit>(isError: true);
        }
        
        _logger.LogInformation("Weather condition(s) was (were) parsed");
        return new CommandResult<Unit>();
    }
}