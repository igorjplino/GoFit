using GoFit.Domain.Entities;

namespace GoFit.Infrastructure.Contexts;

public static class GoFitDbContextInitialise
{
    public static void Seed(this GoFitDbContext context)
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
    }
}
