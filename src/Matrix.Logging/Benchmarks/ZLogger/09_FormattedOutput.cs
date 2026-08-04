using Microsoft.Extensions.Logging;
using ZLogger;
// ReSharper disable CheckNamespace
namespace Matrix.Logging.Benchmarks;

public partial class FormattedOutput
{
    private FormattedOutputSink _zloggerOutput = null!;
    private ILoggerFactory _zloggerFactory = null!;
    private ILogger _zloggerLogger = null!;

    [GlobalSetup(Target = nameof(ZLogger))]
    public void SetupZLogger()
    {
        _zloggerOutput = new FormattedOutputSink();
        _zloggerFactory = LoggerFactory.Create(builder =>
        {
            builder.ClearProviders();
            builder.SetMinimumLevel(LogLevel.Information);
            builder.AddZLoggerLogProcessor(options =>
            {
                options.UsePlainTextFormatter(formatter => formatter.SetPrefixFormatter(
                    $"{0:yyyy-MM-dd HH:mm:ss,fff} {1:short} {2} - ",
                    (in template, in info) => template.Format(info.Timestamp.Local.DateTime, info.LogLevel, info.Category)));
                return new ZLoggerFormattedOutputProcessor(
                    _zloggerOutput,
                    options.CreateFormatter());
            });
        });
        _zloggerLogger = _zloggerFactory.CreateLogger(LoggingData.Category);
    }

    [GlobalCleanup(Target = nameof(ZLogger))]
    public void CleanupZLogger()
    {
        _zloggerFactory.Dispose();
        Verify(LibraryCatalog.ZLogger, _zloggerOutput);
    }

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.ZLogger)]
    public void ZLogger() => _zloggerLogger.ZLogInformation($"Formatted event");
}
