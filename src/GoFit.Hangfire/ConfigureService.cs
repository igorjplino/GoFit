using GoFit.Hangfire.Jobs;
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
            options.UseSimpleAssemblyNameTypeSerializer();
            options.UseRecommendedSerializerSettings();
            options.UseSqlServerStorage(configuration.GetConnectionString("HangfireDb"));

            options.UseFilter(new AutomaticRetryAttribute
            {
                Attempts = 5,
                DelaysInSeconds = new[] { 10, 60, 300, 300, 300 },
                OnAttemptsExceeded = AttemptsExceededAction.Fail
            });
        });
        
        services.AddHangfireServer();
        
        services.AddSingleton<SendMinuteNotificationJob>();
        
        return services;
    }
}