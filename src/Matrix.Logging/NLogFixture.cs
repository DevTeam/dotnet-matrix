using NLog;
using NLog.Config;
using NLog.Targets;
using NLog.Targets.Wrappers;

namespace Matrix.Logging;

internal sealed class NLogFixture : IDisposable
{
    public NLogFixture(LogLevel minimumLevel, bool buffered = false)
    {
        Factory = new LogFactory();
        Sink = new NLogCaptureTarget();
        Target target = Sink;
        if (buffered)
        {
            target = new AsyncTargetWrapper(
                Sink,
                LoggingData.BufferedCapacity,
                AsyncTargetWrapperOverflowAction.Block);
        }

        var configuration = new LoggingConfiguration();
        configuration.AddRule(minimumLevel, LogLevel.Fatal, target, LoggingData.Category);
        Factory.Configuration = configuration;
        Logger = Factory.GetLogger(LoggingData.Category);
    }

    public NLogCaptureTarget Sink { get; }

    public Logger Logger { get; }

    private LogFactory Factory { get; }

    public void Flush() => Factory.Flush(TimeSpan.FromSeconds(5));

    public void Dispose() => Factory.Dispose();
}
