using log4net.Appender;
using log4net.Config;
using log4net.Core;
using log4net.Repository.Hierarchy;

namespace Matrix.Logging.Aot;

internal static class AotProbe
{
    public const string Library = "log4net";

    public const int ExpectedEvents = 1;

    /// <summary>
    /// Configures log4net in code and delivers one Information event to an in-memory appender.
    /// Mirrors how <c>Matrix.Logging.Log4NetFixture</c> configures the library, minus anything
    /// the matrix owns, so that what is probed is the library's own behaviour under Native AOT.
    /// </summary>
    public static int Run()
    {
        var name = $"Matrix.Logging.Aot.{Guid.NewGuid():N}";
        var repository = (Hierarchy)log4net.LogManager.CreateRepository(name);
        var appender = new CountingAppender();
        appender.ActivateOptions();
        BasicConfigurator.Configure(repository, appender);
        repository.Root.Level = Level.Info;
        repository.Configured = true;

        log4net.LogManager.GetLogger(name, "Matrix.Logging.Aot").Info("probe");
        log4net.LogManager.ShutdownRepository(name);
        return appender.Count;
    }

    private sealed class CountingAppender : AppenderSkeleton
    {
        public int Count { get; private set; }

        protected override void Append(LoggingEvent loggingEvent) => Count++;
    }
}
