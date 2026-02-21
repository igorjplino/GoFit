using GoFit.Application.Interfaces.Jobs;

namespace GoFit.Hangfire.Jobs;

public class SendNotificationJob : ISendNotificationJob
{
    public Task SendMinuteNotification()
    {
        Console.WriteLine("Sending notification...");
        return Task.CompletedTask;
    }
}