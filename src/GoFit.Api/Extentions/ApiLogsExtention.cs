namespace GoFit.Api.Extentions;

public static partial class ApiLogsExtention
{
    [LoggerMessage(
        Level = LogLevel.Warning,
        Message = "Exception not mapped for fail response")]
    public static partial void NotMappedFailResponse(
        this ILogger logger, Exception ex);

    [LoggerMessage(
        Level = LogLevel.Information,
        Message = "Request received '{method} {path}'")]
    public static partial void RequestReceived(
        this ILogger logger, string method, string path);

    [LoggerMessage(
        Level = LogLevel.Information,
        Message = "Request '{method} {path}' finished and took {duration} ms")]
    public static partial void DurationRequest(
        this ILogger logger, string method, string path, long duration);
}
