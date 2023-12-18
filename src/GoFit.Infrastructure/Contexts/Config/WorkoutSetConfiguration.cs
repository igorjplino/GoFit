using GoFit.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GoFit.Infrastructure.Contexts.Config;

public class WorkoutSetConfiguration : IEntityTypeConfiguration<WorkoutSet>
{
    public void Configure(EntityTypeBuilder<WorkoutSet> builder)
    {
        builder.ToTable("WorkoutSets");

        builder.HasKey(o => o.Id);

        builder.Property(o => o.Id)
            .ValueGeneratedOnAdd();

        builder.Property(x => x.WarmUp)
            .IsRequired();

        builder.Property(x => x.UntilFailure)
            .IsRequired();

        builder.Property(x => x.ResetTime)
            .IsRequired();

        builder.Property(x => x.Order)
            .IsRequired();
    }
}
