using Microsoft.Extensions.Logging;
using ZLogger;

namespace Matrix.Logging;

internal sealed class ZLoggerFixture : IDisposable
{
    public ZLoggerFixture(LogLevel minimumLevel)
    {
        Sink = new ZLoggerCaptureProcessor();
        Factory = LoggerFactory.Create(builder =>
        {
            builder.ClearProviders();
            builder.SetMinimumLevel(minimumLevel);
            builder.AddZLoggerLogProcessor(Sink);
        });
        Logger = Factory.CreateLogger(LoggingData.Category);
    }

    public ZLoggerCaptureProcessor Sink { get; }

    public ILogger Logger { get; }

    private ILoggerFactory Factory { get; }

    public void Dispose() => Factory.Dispose();
}
