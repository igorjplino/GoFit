using GoFit.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GoFit.Infrastructure.Contexts.Config;

public class WorkoutTrackingConfiguration : IEntityTypeConfiguration<WorkoutTracking>
{
    public void Configure(EntityTypeBuilder<WorkoutTracking> builder)
    {
        builder.ToTable("WorkoutsTracking");

        builder.HasKey(o => o.Id);

        builder.Property(o => o.Id)
            .ValueGeneratedOnAdd();

        builder.HasOne(o => o.Workout)
            .WithMany()
            .HasForeignKey(o => o.WorkoutId)
            .IsRequired();

        builder.HasMany(o => o.Sets)
            .WithOne(o => o.WorkoutTracking)
            .HasForeignKey(o => o.WorkoutTrackingId)
            .IsRequired();
    }
}
