using ZeroLog;
using ZeroLog.Appenders;
using ZeroLog.Configuration;
using ZeroLog.Formatting;

namespace Matrix.Logging.Aot;

internal static class AotProbe
{
    public const string Library = "ZeroLog";

    public const int ExpectedEvents = 1;

    /// <summary>
    /// Configures ZeroLog in code and delivers one Information event to an in-memory appender.
    /// </summary>
    /// <remarks>
    /// The appending strategy is synchronous, so the event has already reached the appender by
    /// the time the session is disposed and the count can be read without a flush.
    /// </remarks>
    public static int Run()
    {
        var sink = new CountingAppender();
        var configuration = new ZeroLogConfiguration
        {
            AutoRegisterEnums = false,
            AppendingStrategy = AppendingStrategy.Synchronous
        };
        configuration.RootLogger.Level = LogLevel.Info;
        configuration.RootLogger.Appenders.Add(sink);
        using (LogManager.Initialize(configuration))
        {
            LogManager.GetLogger("Matrix.Logging.Aot").Info("probe");
        }

        LogManager.Shutdown();
        return sink.Count;
    }

    private sealed class CountingAppender : Appender
    {
        private int _count;

        public int Count => Volatile.Read(ref _count);

        public override void WriteMessage(LoggedMessage message) =>
            Interlocked.Increment(ref _count);
    }
}
