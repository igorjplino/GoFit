﻿using GoFit.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GoFit.Infrastructure.Contexts.Config;

public class WorkoutSetTrackingConfiguration : IEntityTypeConfiguration<WorkoutSetTracking>
{
    public void Configure(EntityTypeBuilder<WorkoutSetTracking> builder)
    {
        builder.ToTable("WorkoutSetsTracking");

        builder.HasKey(o => o.Id);

        builder.Property(o => o.Id)
            .ValueGeneratedOnAdd();

        builder.Property(x => x.Repetitions)
            .IsRequired();

        builder.Property(x => x.Weight)
            .IsRequired();

        builder.Property(x => x.Order)
            .IsRequired();
    }
}
