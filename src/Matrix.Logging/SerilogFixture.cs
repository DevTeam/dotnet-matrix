using Serilog;
using Serilog.Core;
using Serilog.Events;

namespace Matrix.Logging;

internal sealed class SerilogFixture : IDisposable
{
    public SerilogFixture(LogEventLevel minimumLevel, bool buffered = false)
    {
        Sink = new SerilogCaptureSink();
        var configuration = new LoggerConfiguration()
            .MinimumLevel.Is(minimumLevel)
            .Enrich.FromLogContext();
        Logger = buffered
            ? configuration.WriteTo.Async(
                    sinks => sinks.Sink(Sink),
                    bufferSize: 1024,
                    blockWhenFull: true)
                .CreateLogger()
            : configuration.WriteTo.Sink(Sink).CreateLogger();
    }

    public SerilogCaptureSink Sink { get; }

    public Logger Logger { get; }

    public void Dispose() => Logger.Dispose();
}

