using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace TodoList.Application;
public static class ConfigureService
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddMediatR(cfg => {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            //cfg.AddBehavior<IPipelineBehavior<Ping, Pong>, PingPongBehavior>();
            //cfg.AddOpenBehavior(typeof(GenericBehavior<,>));
        });

        return services;
    }
}
