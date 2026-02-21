namespace GoFit.Application.Interfaces.Jobs;

public interface ISendNotificationJob
{
    Task SendMinuteNotification();
}