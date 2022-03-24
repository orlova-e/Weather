﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Weather.Domain.Entities;

namespace Weather.Infrastructure.Mappings;

public class WeatherConditionConfiguration : IEntityTypeConfiguration<WeatherCondition>
{
    public void Configure(EntityTypeBuilder<WeatherCondition> builder)
    {
        builder
            .Property(x => x.DateTime)
            .HasColumnType("timestamp without time zone")
            .IsRequired();

        builder
            .Property(x => x.Temperature)
            .HasColumnType("double precision")
            .IsRequired();
        
        builder
            .Property(x => x.RelativeHumidity)
            .HasColumnType("double precision")
            .IsRequired();
        
        builder
            .Property(x => x.DewPoint)
            .HasColumnType("double precision")
            .IsRequired();
        
        builder
            .Property(x => x.AtmosphericPressure)
            .HasColumnType("integer")
            .IsRequired();
        
        builder
            .Property(x => x.WindDirection)
            .HasColumnType("integer")
            .IsRequired();
        
        builder
            .Property(x => x.WindSpeed)
            .HasColumnType("integer")
            .IsRequired();
        
        builder
            .Property(x => x.CloudCover)
            .HasColumnType("double precision")
            .IsRequired();
        
        builder
            .Property(x => x.LowerCloudLimit)
            .HasColumnType("integer")
            .IsRequired();
        
        builder
            .Property(x => x.HorizontalVisibility)
            .HasColumnType("integer")
            .IsRequired(false);
        
        builder
            .Property(x => x.Phenomena)
            .HasColumnType("text")
            .IsRequired(false);

        builder
            .HasOne(x => x.City)
            .WithMany(x => x.WeatherConditions)
            .HasForeignKey(x => x.CityId);
    }
}