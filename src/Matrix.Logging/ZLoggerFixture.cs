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
            // The direct-instance overload has no access to ZLoggerOptions, and
            // scope properties are only attached when IncludeScopes is set.
            builder.AddZLoggerLogProcessor(options =>
            {
                options.IncludeScopes = true;
                return Sink;
            });
        });
        Logger = Factory.CreateLogger(LoggingData.Category);
    }

    public ZLoggerCaptureProcessor Sink { get; }

    public ILogger Logger { get; }

    private ILoggerFactory Factory { get; }

    public void Dispose() => Factory.Dispose();
}
