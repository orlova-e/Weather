using FluentValidation;
using Microsoft.Extensions.Options;
using Weather.Services.Files;
using Weather.Services.MappedModels;
using Weather.Services.Models;
using Weather.Web.Models.Configuration;
using Weather.Web.Models.Weather;

namespace Weather.Web.Services.Validation.Weather;

public class LoadWeatherDtoValidator : AbstractValidator<LoadWeatherDto>
{
    private readonly IOptions<ValidationOptions> _options;
    private readonly IFileService<WeatherMapModel> _fileService;

    public LoadWeatherDtoValidator(
        IOptions<ValidationOptions> options,
        IFileService<WeatherMapModel> fileService)
    {
        _options = options;
        _fileService = fileService;

        RuleFor(x => x.Files)
            .NotEmpty();

        RuleForEach(x => x.Files)
            .NotEmpty()
            .Must(IsFileExtensionValid)
            .WithMessage(ValidationMessages.InvalidFileExtension)
            .CustomAsync(CanBeWritten);
    }

    private bool IsFileExtensionValid(IFormFile file)
    {
        var extension = Path.GetExtension(file.FileName);
        return _options.Value.File.Extensions.Contains(extension);
    }

    private async Task CanBeWritten(
        IFormFile file,
        ValidationContext<LoadWeatherDto> context,
        CancellationToken cancellationToken)
    {
        await using var stream = file.OpenReadStream();
        var error =  _fileService.Validate(stream, 4);
        
        if (error is FileReadMessages.Normal)
            return;

        var failureMessage = error switch
        {
            FileReadMessages.UnableToMap => ValidationMessages.UnableToMapFile,
            FileReadMessages.UnableToOpen => ValidationMessages.UnableToOpenFile,
            FileReadMessages.UnableToRead => ValidationMessages.UnableToReadFile,
            { } => string.Empty
        };

        context.AddFailure(failureMessage);
    }
}