using Microsoft.Extensions.Logging;

namespace Matrix.Logging;

internal sealed class MicrosoftLoggingFixture : IDisposable
{
    public MicrosoftLoggingFixture(LogLevel minimumLevel)
    {
        Sink = new MicrosoftCaptureLoggerProvider();
        Factory = LoggerFactory.Create(builder =>
        {
            builder.ClearProviders();
            builder.SetMinimumLevel(minimumLevel);
            builder.AddProvider(Sink);
        });
        Logger = Factory.CreateLogger(LoggingData.Category);
    }

    public MicrosoftCaptureLoggerProvider Sink { get; }

    public ILogger Logger { get; }

    private ILoggerFactory Factory { get; }

    public void Dispose() => Factory.Dispose();
}

