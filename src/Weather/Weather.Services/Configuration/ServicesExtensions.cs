using Microsoft.Extensions.DependencyInjection;
using Weather.Services.Data;
using Weather.Services.Files;

namespace Weather.Services.Configuration;

public static class ServicesExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services
            .AddTransient<IInfoService, InfoService>()
            .AddSingleton(typeof(IFileService<>), typeof(ExcelService<>));

        return services;
    }
}