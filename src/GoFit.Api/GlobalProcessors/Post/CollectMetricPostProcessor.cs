using FastEndpoints;
using GoFit.Api.Extensions;
using GoFit.Api.GlobalProcessors.Models;

namespace GoFit.Api.GlobalProcessors.Pre;

public class CollectMetricPostProcessor : GlobalPostProcessor<MetricState>, IGlobalPostProcessor
{
    private readonly ILogger<CollectMetricPostProcessor> _logger;

    public CollectMetricPostProcessor(ILogger<CollectMetricPostProcessor> logger)
    {
        _logger = logger;
    }

    public override Task PostProcessAsync(IPostProcessorContext context, MetricState state, CancellationToken ct)
    {
        _logger.DurationRequest(context.HttpContext.Request.Method, context.HttpContext.Request.Path, state.DurationMillis);

        return Task.CompletedTask;
    }
}
