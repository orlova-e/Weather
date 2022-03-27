using FluentValidation;
using MediatR;
using Weather.Web.Services.Commands.Weather;
using Weather.Web.Services.Validation.Weather;

namespace Weather.Web.Services.Configuration;

public static class WebExtensions
{
    public static IServiceCollection AddWebServices(this IServiceCollection services)
    {
        services
            .AddValidatorsFromAssemblyContaining<LoadWeatherDtoValidator>()
            .AddAutoMapper(typeof(Program).Assembly)
            .AddMediatR(typeof(GetWeatherConditionsRequest).Assembly);
        
        return services;
    }
}