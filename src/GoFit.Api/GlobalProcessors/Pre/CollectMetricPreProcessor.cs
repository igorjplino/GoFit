using FastEndpoints;
using GoFit.Api.Extensions;
using GoFit.Api.GlobalProcessors.Models;

namespace GoFit.Api.GlobalProcessors.Pre;

public class CollectMetricPreProcessor : GlobalPreProcessor<MetricState>, IGlobalPreProcessor
{
    private readonly ILogger<CollectMetricPreProcessor> _logger;

    public CollectMetricPreProcessor(ILogger<CollectMetricPreProcessor> logger)
    {
        _logger = logger;
    }

    public override Task PreProcessAsync(IPreProcessorContext context, MetricState state, CancellationToken ct)
    {
        _logger.RequestReceived(context.HttpContext.Request.Method, context.HttpContext.Request.Path);

        return Task.CompletedTask;
    }
}
