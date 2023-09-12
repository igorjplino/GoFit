using GoFit.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GoFit.Infrastructure.Contexts.Config;

public class WorkoutConfiguration : IEntityTypeConfiguration<Workout>
{
    public void Configure(EntityTypeBuilder<Workout> builder)
    {
        builder.ToTable("Workout");

        builder.HasKey(o => o.Id);

        builder.Property(o => o.Id)
            .ValueGeneratedOnAdd();

        builder.HasMany(o => o.Sets)
            .WithOne(o => o.Workout)
            .HasForeignKey(o => o.WorkoutId)
            .IsRequired();
    }
}
