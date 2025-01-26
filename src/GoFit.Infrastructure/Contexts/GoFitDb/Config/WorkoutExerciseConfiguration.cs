using GoFit.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GoFit.Infrastructure.Contexts.Config;

public class WorkoutExerciseConfiguration : IEntityTypeConfiguration<WorkoutExercise>
{
    public void Configure(EntityTypeBuilder<WorkoutExercise> builder)
    {
        builder.ToTable("WorkoutExercises");

        builder.HasKey(o => o.Id);

        builder.Property(o => o.Id)
            .ValueGeneratedOnAdd();

        builder.Property(x => x.Order)
            .IsRequired();

        builder.HasOne(o => o.Exercise)
            .WithMany()
            .HasForeignKey(o => o.ExerciseId)
            .IsRequired();

        builder.HasMany(o => o.Sets)
            .WithOne(o => o.WorkoutExercise)
            .HasForeignKey(o => o.WorkoutExerciseId)
            .IsRequired();

    }
}
