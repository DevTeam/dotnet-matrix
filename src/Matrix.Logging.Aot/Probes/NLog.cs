using NLog;
using NLog.Config;
using NLog.Targets;

namespace Matrix.Logging.Aot;

internal static class AotProbe
{
    public const string Library = "NLog";

    public const int ExpectedEvents = 1;

    /// <summary>
    /// Configures NLog in code and delivers one Information event to an in-memory target.
    /// </summary>
    public static int Run()
    {
        using var factory = new LogFactory();
        var target = new CountingTarget();
        var configuration = new LoggingConfiguration();
        configuration.AddRule(LogLevel.Info, LogLevel.Fatal, target, "Matrix.Logging.Aot");
        factory.Configuration = configuration;

        factory.GetLogger("Matrix.Logging.Aot").Info("probe");
        return target.Count;
    }

    private sealed class CountingTarget : TargetWithLayout
    {
        public int Count { get; private set; }

        protected override void Write(NLog.LogEventInfo logEvent) => Count++;
    }
}
