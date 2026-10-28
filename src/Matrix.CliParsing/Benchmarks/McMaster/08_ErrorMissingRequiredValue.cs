using McMaster.Extensions.CommandLineUtils;
// ReSharper disable CheckNamespace
namespace Matrix.CliParsing.Benchmarks;

public partial class ErrorMissingRequiredValue
{
    private readonly CommandLineApplication<McMasterReportOptions> _mcApplication = McMasterConfiguration.Create<McMasterReportOptions>();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.McMasterCommandLineUtils)]
    public ParseOutcome McMasterCommandLineUtils()
    {
        var result = McMasterConfiguration.ReadFailure(_mcApplication, _args);
        CliChecks.RequireFailure(LibraryCatalog.McMasterCommandLineUtils, result);
        return result;
    }
}
