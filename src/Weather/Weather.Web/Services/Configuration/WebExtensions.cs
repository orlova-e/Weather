using System.Globalization;
using FluentValidation.AspNetCore;
using MediatR;
using Weather.Web.Models.Configuration;
using Weather.Web.Services.Commands.Weather;
using Weather.Web.Services.Validation.Weather;

namespace Weather.Web.Services.Configuration;

public static class WebExtensions
{
    public static IServiceCollection AddWebServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .AddMvc()
            .AddFluentValidation(f =>
                f.RegisterValidatorsFromAssemblyContaining<LoadWeatherDtoValidator>()).Services
            .AddAutoMapper(typeof(Program).Assembly)
            .AddMediatR(typeof(GetWeatherConditionsRequest).Assembly)
            .Configure<FileSettings>(configuration.GetSection(nameof(FileSettings)));
        
        return services;
    }
}