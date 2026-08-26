using System.Globalization;
using Microsoft.Extensions.Logging;
using ZLogger;
// ReSharper disable CheckNamespace
namespace Matrix.Logging.Benchmarks;

public partial class TemplateRendering
{
    private ZLoggerFixture _zlogger = null!;
    private ILogger _zloggerLogger = null!;
    private CultureInfo _originalCulture = null!;

    [GlobalSetup(Target = nameof(ZLogger))]
    public void SetupZLogger()
    {
        _zlogger = new ZLoggerFixture(LogLevel.Information);
        _zloggerLogger = _zlogger.Logger;
        // ZLogger's interpolated-string API has no per-call format provider, so
        // invariant formatting has to come from the ambient culture instead.
        _originalCulture = CultureInfo.CurrentCulture;
        CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
    }

    [GlobalCleanup(Target = nameof(ZLogger))]
    public void CleanupZLogger()
    {
        CultureInfo.CurrentCulture = _originalCulture;
        _zlogger.Dispose();
    }

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.ZLogger)]
    public void ZLogger()
    {
        var amount = LoggingData.Amount;
        var customer = LoggingData.Customer;
        _zloggerLogger.ZLogInformation($"Total {amount:0.00} for {customer}");
        LoggingChecks.Template(LibraryCatalog.ZLogger, _zlogger.Sink.Last);
    }
}
