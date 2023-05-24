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
        if (configuration.GetValue<bool>("UseInMemoryDatabase"))
        {
            services.AddDbContext<GoFitDbContext>(options =>
                options.UseInMemoryDatabase("GoFitDb"));
        }
        else
        {
            //TODO: connect to a real database
        }

        services.AddSingleton<IWorkoutRepository, WorkoutRepository>();
        services.AddSingleton<IWorkoutPlanRepository, WorkoutPlanRepository>();

        return services;
    }
}
