using GoFit.Application.Interfaces.Jobs;
using GoFit.Hangfire.Jobs;
using GoFit.Hangfire.Recurring;
using Hangfire;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GoFit.Hangfire;

public static class ConfigureService
{
    public static IServiceCollection AddHangfireServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHangfire(options =>
        {
            options.SetDataCompatibilityLevel(CompatibilityLevel.Version_180);
            options.UseSimpleAssemblyNameTypeSerializer();
            options.UseRecommendedSerializerSettings();
            options.UseSqlServerStorage(configuration.GetConnectionString("HangfireDb"));
        });
        
        services.AddHangfireServer();
        
        services.AddScoped<ISendNotificationJob, SendNotificationJob>();
        
        return services;
    }
}