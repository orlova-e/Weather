using Microsoft.EntityFrameworkCore;
using Weather.Domain.Entities;

namespace Weather.Infrastructure.Mappings;

public class Context : DbContext
{
    public DbSet<WeatherCondition> WeatherConditions { get; set; }
    
    public Context(DbContextOptions<Context> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(WeatherConditionConfiguration).Assembly);
    }
}