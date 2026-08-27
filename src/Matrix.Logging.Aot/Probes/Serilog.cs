using Serilog;
using Serilog.Core;
using Serilog.Events;

namespace Matrix.Logging.Aot;

internal static class AotProbe
{
    public const string Library = "Serilog";

    public const int ExpectedEvents = 1;

    /// <summary>
    /// Configures Serilog in code and delivers one Information event to an in-memory sink.
    /// </summary>
    public static int Run()
    {
        var sink = new CountingSink();
        using var logger = new LoggerConfiguration()
            .MinimumLevel.Is(LogEventLevel.Information)
            .Enrich.FromLogContext()
            .WriteTo.Sink(sink)
            .CreateLogger();

        logger.Information("probe");
        return sink.Count;
    }

    private sealed class CountingSink : ILogEventSink
    {
        public int Count { get; private set; }

        public void Emit(LogEvent logEvent) => Count++;
    }
}
