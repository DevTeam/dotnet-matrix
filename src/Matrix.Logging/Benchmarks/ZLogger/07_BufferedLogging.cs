using Microsoft.Extensions.Logging;
using ZLogger;
// ReSharper disable CheckNamespace
namespace Matrix.Logging.Benchmarks;

public partial class BufferedLogging
{
    private ILoggerFactory _zloggerFactory = null!;
    private ZLoggerBufferedSink _zloggerSink = null!;
    private ILogger _zloggerLogger = null!;

    [GlobalSetup(Target = nameof(ZLogger))]
    public void SetupZLogger()
    {
        _zloggerSink = new ZLoggerBufferedSink();
        _zloggerFactory = LoggerFactory.Create(builder =>
        {
            builder.ClearProviders();
            builder.SetMinimumLevel(LogLevel.Information);
            // Only the stream/file/console providers dispatch through ZLogger's
            // own background buffer; AddZLoggerLogProcessor does not, regardless
            // of these options.
            builder.AddZLoggerStream(_zloggerSink, options =>
            {
                options.UsePlainTextFormatter();
                options.BackgroundBufferCapacity = LoggingData.BufferedCapacity;
                options.FullMode = BackgroundBufferFullMode.Block;
            });
        });
        _zloggerLogger = _zloggerFactory.CreateLogger(LoggingData.Category);
    }

    [GlobalCleanup(Target = nameof(ZLogger))]
    public void CleanupZLogger()
    {
        _zloggerFactory.Dispose();
        LoggingChecks.Buffered(
            LibraryCatalog.ZLogger,
            _zloggerSink.Count,
            _zloggerSink.Last);
    }

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.ZLogger)]
    public void ZLogger() =>
        _zloggerLogger.ZLogInformation($"Buffered event");
}
