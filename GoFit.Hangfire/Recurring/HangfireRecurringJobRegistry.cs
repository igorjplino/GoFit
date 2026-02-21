using GoFit.Application.Interfaces.Jobs;
using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace GoFit.Hangfire.Recurring;

public static class HangfireRecurringJobRegistry
{
    public static void RegisterAllRecurringJob(this WebApplication app)
    {
        using (var scope = app.Services.CreateScope())
        {
            var recurring = scope.ServiceProvider.GetRequiredService<IRecurringJobManager>();

            recurring.AddOrUpdate<ISendNotificationJob>(
                "notification-every-minute",
                x => x.SendMinuteNotification(),
                Cron.Minutely);
        }
    }
}