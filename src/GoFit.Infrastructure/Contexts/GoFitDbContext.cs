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

        modelBuilder.Entity<Workout>(model =>
        {
            model.ToTable("Workout");

            model.HasKey(o => o.Id);

            model.Property(o => o.Id)
                .ValueGeneratedOnAdd();
        });

        modelBuilder.Entity<WorkoutSet>(model =>
        {
            model.ToTable("WorkoutSet");

            model.HasKey(o => o.Id);

            model.Property(o => o.Id)
                .ValueGeneratedOnAdd();
        });

        modelBuilder.Entity<WorkoutPlan>(model =>
        {
            model.ToTable("WorkoutPlan");

            model.HasKey(o => o.Id);

            model.Property(o => o.Id)
                .ValueGeneratedOnAdd();

            model.Ignore(o => o.WorkoutsId);
        });
    }
}
