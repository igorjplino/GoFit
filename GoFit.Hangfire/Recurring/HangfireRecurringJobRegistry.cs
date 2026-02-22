using GoFit.Hangfire.Jobs;
using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace GoFit.Hangfire.Recurring;

public static class HangfireRecurringJobRegistry
{
    public static void RegisterAllRecurringJob(this WebApplication app)
    {
        RecurringJob.AddOrUpdate<SendMinuteNotificationJob>(
            "minute-notification",
            x => x.ExecuteAsync(),
            Cron.Hourly);
    }
    
    public static void RegisterAllRecurringJobByDI(this WebApplication app)
    {
        using (var scope = app.Services.CreateScope())
        {
            var recurring = scope.ServiceProvider.GetRequiredService<IRecurringJobManager>();
            
            // This implementations requires to put the AutomaticRetry in Interface
            recurring.AddOrUpdate<SendMinuteNotificationJob>(
                "minute-notification",
                x => x.ExecuteAsync(),
                Cron.Hourly);
        }
    }
}