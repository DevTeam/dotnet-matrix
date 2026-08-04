using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Core;
using log4net.Repository.Hierarchy;

namespace Matrix.Logging;

internal sealed class Log4NetFixture : IDisposable
{
    private readonly BufferingForwardingAppender? _buffer;

    public Log4NetFixture(Level minimumLevel, bool buffered = false)
    {
        var repositoryName = $"{LoggingData.Category}.{Guid.NewGuid():N}";
        Repository = (Hierarchy)LogManager.CreateRepository(repositoryName);
        Sink = new();
        Sink.ActivateOptions();
        IAppender appender = Sink;
        if (buffered)
        {
            _buffer = new()
            {
                BufferSize = LoggingData.BufferedCapacity,
                Lossy = false,
                // log4net defaults to FixFlags.All, which captures caller location (a stack walk)
                // and the Windows identity (a local security authority lookup) for every buffered
                // event. NLog's AsyncTargetWrapper and Serilog's WriteTo.Async capture neither, so
                // the default would charge log4net for work the other arms never do. Partial is the
                // set log4net's own FixFlags documentation recommends for performance, and it still
                // fixes everything this feature validates: message, level, exception and properties.
                Fix = FixFlags.Partial
            };
            _buffer.AddAppender(Sink);
            _buffer.ActivateOptions();
            appender = _buffer;
        }

        BasicConfigurator.Configure(Repository, appender);
        Repository.Root.Level = minimumLevel;
        Repository.Configured = true;
        Logger = LogManager.GetLogger(repositoryName, LoggingData.Category);
    }

    public Log4NetCaptureAppender Sink { get; }

    public ILog Logger { get; }

    private Hierarchy Repository { get; }

    public void Flush() => _buffer?.Flush();

    public void Dispose()
    {
        _buffer?.Close();
        LogManager.ShutdownRepository(Repository.Name);
    }
}
