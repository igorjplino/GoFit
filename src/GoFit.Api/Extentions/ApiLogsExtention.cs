namespace GoFit.Api.Extentions;

public static partial class ApiLogsExtention
{
    [LoggerMessage(
        Level = LogLevel.Warning,
        Message = "Exception not mapped for fail response")]
    public static partial void NotMappedFailResponse(
        this ILogger logger, Exception ex);
}
