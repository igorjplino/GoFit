using GoFit.Infrastructure.Models;
using Hangfire;
using Microsoft.Extensions.Options;

namespace GoFit.Hangfire.Jobs;

public class SendMinuteNotificationJob
{
    private readonly string _goFitDb;
    
    public SendMinuteNotificationJob(IOptions<Connections> options)
    {
        _goFitDb = options.Value.GoFitDb;
    }
    
    [AutomaticRetry(Attempts = 3, DelaysInSeconds = new[] { 10, 30, 60 })]
    public Task ExecuteAsync()
    {
        Console.WriteLine("Sending notification...");
        
        return Task.CompletedTask;
    }
}