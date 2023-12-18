using GoFit.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GoFit.Infrastructure.Contexts.Config;

public class WorkoutPlanConfiguration : IEntityTypeConfiguration<WorkoutPlan>
{
    public void Configure(EntityTypeBuilder<WorkoutPlan> builder)
    {
        builder.ToTable("WorkoutsPlan");

        builder.HasKey(o => o.Id);

        builder.Property(o => o.Id)
            .ValueGeneratedOnAdd();

        builder.Property(o => o.Title)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(o => o.Description)
            .HasMaxLength(300);

        builder.HasMany(o => o.Workouts)
            .WithOne(o => o.WorkoutPlan)
            .HasForeignKey(o => o.WorkoutPlanId)
            .IsRequired();
    }
}
