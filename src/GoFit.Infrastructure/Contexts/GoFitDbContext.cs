using GoFit.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GoFit.Infrastructure.Contexts;
public class GoFitDbContext : DbContext
{
    public DbSet<Exercise> Exercises { get; set; }
    public DbSet<Workout> Workouts { get; set; }
    public DbSet<WorkoutSet> WorkoutsSet { get; set; }
    public DbSet<WorkoutPlan> WorkoutsPlan { get; set; }

    public GoFitDbContext(DbContextOptions<GoFitDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Exercise>(model => 
        {
            model.ToTable("Exercises");

            model.HasKey(o => o.Id);

            model.Property(o => o.Id)
                .ValueGeneratedOnAdd();
        });
    }
}
