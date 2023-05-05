using Microsoft.Extensions.DependencyInjection;
using GoFit.Application.Interfaces;
using GoFit.Infrastructure.Repositories;

namespace GoFit.Infrastructure;
public static class ConfigureService
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddSingleton<ITodoItemRepository, TodoItemRepository>();
        services.Decorate<ITodoItemRepository, CachedTodoItemRepository>();

        services.AddSingleton<IWorkoutRepository, WorkoutRepository>();
        services.AddSingleton<IWorkoutPlanRepository, WorkoutPlanRepository>();

        return services;
    }
}
