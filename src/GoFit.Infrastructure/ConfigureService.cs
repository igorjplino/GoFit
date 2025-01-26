using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using GoFit.Application.Interfaces;
using GoFit.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using GoFit.Infrastructure.Contexts;
using System;
using GoFit.Application.Models;
using GoFit.Domain.Entities.Identity;
using GoFit.Infrastructure.Contexts.GoFitDb;
using GoFit.Infrastructure.Contexts.IdentityDb;
using GoFit.Infrastructure.Models;
using Microsoft.AspNetCore.Identity;

namespace GoFit.Infrastructure;
public static class ConfigureService
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<Connections>(configuration.GetSection("ConnectionStrings"));
        
        services.AddDbContext<GoFitDbContext>(options => 
        {
            options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            options.UseSqlServer(configuration.GetConnectionString("GoFitDb"));
        });

        services.AddDbContext<AppIdentityDbContext>(options =>
        {
            options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            options.UseSqlServer(configuration.GetConnectionString("IdentityDb"));
        });
        
        services
            .BuildServiceProvider()
            .GetRequiredService<GoFitDbContext>()
            .ApplyMigration()
            .Seed();

        services.AddScoped<IWorkoutRepository, WorkoutRepository>();
        services.AddScoped<IWorkoutTrackingRepository, WorkoutTrackingRepository>();
        services.AddScoped<IWorkoutPlanRepository, WorkoutPlanRepository>();
        services.AddScoped<IExerciseRepository, ExerciseRepository>();
        services.AddScoped<IAthleteRepository, AthleteRepository>();

        return services;
    }
}
