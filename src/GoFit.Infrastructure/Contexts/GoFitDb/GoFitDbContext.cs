using System.Reflection;
using GoFit.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GoFit.Infrastructure.Contexts.GoFitDb;

public class GoFitDbContext : DbContext
{
    public DbSet<Athlete> Athletes { get; set; }
    public DbSet<Exercise> Exercises { get; set; }
    public DbSet<Workout> Workouts { get; set; }
    public DbSet<WorkoutSet> WorkoutsSet { get; set; }
    public DbSet<WorkoutPlan> WorkoutsPlan { get; set; }
    public DbSet<WorkoutExercise> WorkoutsExercises { get; set; }
    public DbSet<WorkoutTracking> WorkoutsTracking { get; set; }
    public DbSet<WorkoutSetTracking> WorkoutSetsTracking { get; set; }
    
    public GoFitDbContext(DbContextOptions<GoFitDbContext> options)
        : base(options)
    { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    => optionsBuilder
        .LogTo(Console.WriteLine)
        .EnableSensitiveDataLogging();

}
