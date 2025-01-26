using GoFit.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GoFit.Infrastructure.Contexts.GoFitDb;

public static class GoFitDbContextInitialise
{
    public static GoFitDbContext ApplyMigration(this GoFitDbContext context)
    {
        context.Database.Migrate();

        return context;
    }

    public static GoFitDbContext Seed(this GoFitDbContext context)
    {
        // Default data for tests
        if (!context.Exercises.Any())
        {
            context.Exercises.AddRangeAsync(new List<Exercise>
            {
                new Exercise { Name = "Arnald Press", Description = "" },
                new Exercise { Name = "Alternating Dumbell Bicep Curls", Description = "" },
                new Exercise { Name = "Barbell Bench Press", Description = "" },
                new Exercise { Name = "Bench Dips", Description = "" },
                new Exercise { Name = "Pull Ups", Description = "" }
            });

            context.SaveChanges();
        }

        return context;
    }
}
