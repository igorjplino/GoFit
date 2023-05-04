using Microsoft.Extensions.DependencyInjection;
using TodoList.Application.Interfaces;
using TodoList.Infrastructure.Repositories;

namespace TodoList.Infrastructure;
public static class ConfigureService
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddSingleton<ITodoItemRepository, TodoItemRepository>();
        services.Decorate<ITodoItemRepository, CachedTodoItemRepository>();

        return services;
    }
}
