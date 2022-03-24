using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Weather.Infrastructure.Access;
using Weather.Infrastructure.Mappings;

namespace Weather.Infrastructure.Configuration;

public static class InfrastructureExtensions
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        
        services
            .AddDbContext<Context>(option => option.UseNpgsql(connectionString))
            .AddScoped(typeof(IEntityRepository<>), typeof(Repository<>));

        return services;
    }
}