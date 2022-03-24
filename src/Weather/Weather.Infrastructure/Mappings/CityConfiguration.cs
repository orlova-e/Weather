using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Weather.Domain.Entities;

namespace Weather.Infrastructure.Mappings;

public class CityConfiguration : IEntityTypeConfiguration<City>
{
    public void Configure(EntityTypeBuilder<City> builder)
    {
        builder
            .Property(x => x.Name)
            .HasColumnType("text")
            .IsRequired();
    }
}