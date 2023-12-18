using GoFit.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GoFit.Infrastructure.Contexts.Config;

public class WorkoutConfiguration : IEntityTypeConfiguration<Workout>
{
    public void Configure(EntityTypeBuilder<Workout> builder)
    {
        builder.ToTable("Workouts");

        builder.HasKey(o => o.Id);

        builder.Property(o => o.Id)
            .ValueGeneratedOnAdd();

        builder.Property(o => o.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(o => o.Description)
            .HasMaxLength(300);

        builder.HasMany(o => o.WorkoutExercises)
            .WithOne(o => o.Workout)
            .HasForeignKey(o => o.WorkoutId)
            .IsRequired();
    }
}
