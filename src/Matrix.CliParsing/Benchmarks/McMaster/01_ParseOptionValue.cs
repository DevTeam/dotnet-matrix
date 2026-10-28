using McMaster.Extensions.CommandLineUtils;
// ReSharper disable CheckNamespace
namespace Matrix.CliParsing.Benchmarks;

public partial class ParseOptionValue
{
    private readonly CommandLineApplication<McMasterReportOptions> _mcApplication = McMasterConfiguration.Create<McMasterReportOptions>();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.McMasterCommandLineUtils)]
    public ReportArguments McMasterCommandLineUtils()
    {
        var result = McMasterConfiguration.ReadReport(_mcApplication, _args);
        CliChecks.RequireReport(LibraryCatalog.McMasterCommandLineUtils, result);
        return result;
    }
}
