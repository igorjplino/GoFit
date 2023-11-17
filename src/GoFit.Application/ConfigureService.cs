using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using GoFit.Application.Common.PipelineBehaviours;
using GoFit.Application.EntitiesActions.Exercises.Commands;
using GoFit.Application.Common;
using GoFit.Application.EntitiesActions.WorkoutPlans.Commands;
using GoFit.Application.EntitiesActions.WorkoutsTracking.Commands;

namespace GoFit.Application;
public static class ConfigureService
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddMediatR(cfg => {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            cfg.AddValidation<CreateExerciseCommand, Guid>();
            cfg.AddValidation<CreateWorkoutPlanCommand, Guid>();
            cfg.AddValidation<CreateWorkoutTrackingCommand, Guid>();
        });

        return services;
    }

    private static MediatRServiceConfiguration AddValidation<TRequest, TResponse>(
        this MediatRServiceConfiguration config) where TRequest : notnull
    {
        return config
            .AddBehavior<IPipelineBehavior<TRequest, Result<TResponse>>, ValidationBehaviour<TRequest, TResponse>>();
    }
}
