using Microsoft.Extensions.Logging;

namespace GoFit.Application.Extentions;
public static partial class ApplicationLogsExtention
{
    [LoggerMessage(
        Level = LogLevel.Information,
        Message = "Start execution of the command '{commandName}'")]
    public static partial void ExecutingCommand(
        this ILogger logger, string commandName);

    [LoggerMessage(
        Level = LogLevel.Information,
        Message = "Validating command '{commandName}'")]
    public static partial void ValidatingCommand(
        this ILogger logger, string commandName);

    [LoggerMessage(
        Level = LogLevel.Information,
        Message = "Command '{commandName}' is not valid for execution, total errors found: {totalErrors}")]
    public static partial void CommandIsInvalid(
        this ILogger logger, string commandName, int totalErrors);
}
