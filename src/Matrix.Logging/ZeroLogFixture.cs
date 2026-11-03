using ZeroLog;
using ZeroLog.Appenders;
using ZeroLog.Configuration;

namespace Matrix.Logging;

internal sealed class ZeroLogFixture : IDisposable
{
    private readonly IDisposable _session;

    public ZeroLogFixture(LogLevel minimumLevel, bool buffered = false)
    {
        Sink = new ZeroLogCaptureAppender();
        var configuration = new ZeroLogConfiguration
        {
            AutoRegisterEnums = false,
            LogMessagePoolSize = LoggingData.BufferedCapacity,
            // ZeroLog renders structured values with the invariant culture, so no culture
            // adjustment is needed around the measured call.
            AppendingStrategy = buffered
                ? AppendingStrategy.Asynchronous
                : AppendingStrategy.Synchronous
        };
        configuration.RootLogger.Level = minimumLevel;
        configuration.RootLogger.Appenders.Add(Sink);
        _session = LogManager.Initialize(configuration);
        Logger = LogManager.GetLogger(LoggingData.Category);
    }

    public ZeroLogCaptureAppender Sink { get; }

    public Log Logger { get; }

    /// <summary>
    /// Waits until every event queued by the asynchronous appending strategy reaches the sink.
    /// </summary>
    public static void Flush() => LogManager.Flush();

    public void Dispose()
    {
        _session.Dispose();
        LogManager.Shutdown();
    }
}
