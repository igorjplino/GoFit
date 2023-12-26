using System.Diagnostics;

namespace GoFit.Api.GlobalProcessors.Models;

public class MetricState
{
    private readonly Stopwatch _stopwatch;

    public MetricState()
    {
        _stopwatch = Stopwatch.StartNew();
    }

    public long DurationMillis => _stopwatch.ElapsedMilliseconds;
}
