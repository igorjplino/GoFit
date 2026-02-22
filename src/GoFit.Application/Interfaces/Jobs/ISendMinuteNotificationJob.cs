namespace GoFit.Application.Interfaces.Jobs;

public interface ISendMinuteNotificationJob
{
    Task ExecuteAsync();
}