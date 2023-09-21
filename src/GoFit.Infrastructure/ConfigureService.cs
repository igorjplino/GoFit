using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using GoFit.Application.Interfaces;
using GoFit.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using GoFit.Infrastructure.Contexts;

namespace GoFit.Infrastructure;
public static class ConfigureService
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<GoFitDbContext>(options =>
                options.UseSqlite(configuration.GetConnectionString("GoFitDb")));

        services
            .BuildServiceProvider()
            .GetRequiredService<GoFitDbContext>()
            .Seed();

        services.AddScoped<IWorkoutRepository, WorkoutRepository>();
        services.AddScoped<IWorkoutPlanRepository, WorkoutPlanRepository>();
        services.AddScoped<IExerciseRepository, ExerciseRepository>();

        return services;
    }
}
