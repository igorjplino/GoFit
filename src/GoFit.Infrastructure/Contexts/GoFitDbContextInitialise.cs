using GoFit.Domain.Entities;

namespace GoFit.Infrastructure.Contexts;
public class GoFitDbContextInitialise
{
	private readonly GoFitDbContext _context;

    public GoFitDbContextInitialise(GoFitDbContext context)
    {
        _context = context;
    }

    public async Task SeedAsync()
    {
        // Default data for tests
        if (!_context.Exercises.Any())
        {
            await _context.Exercises.AddRangeAsync(new List<Exercise>
            {
                new Exercise { Name = "Arnald Press", Description = "" },
                new Exercise { Name = "Alternating Dumbell Bicep Curls", Description = "" },
                new Exercise { Name = "Barbell Bench Press", Description = "" },
                new Exercise { Name = "Bench Dips", Description = "" },
                new Exercise { Name = "Pull Ups", Description = "" }
            });

            await _context.SaveChangesAsync();
        }
    }
}
