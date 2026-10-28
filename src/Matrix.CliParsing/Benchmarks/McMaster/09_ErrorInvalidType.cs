using McMaster.Extensions.CommandLineUtils;
// ReSharper disable CheckNamespace
namespace Matrix.CliParsing.Benchmarks;

public partial class ErrorInvalidType
{
    private readonly CommandLineApplication<McMasterCompactOptions> _mcApplication = McMasterConfiguration.Create<McMasterCompactOptions>();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.McMasterCommandLineUtils)]
    public ParseOutcome McMasterCommandLineUtils()
    {
        var result = McMasterConfiguration.ReadCompactFailure(_mcApplication, _args);
        CliChecks.RequireFailure(LibraryCatalog.McMasterCommandLineUtils, result);
        return result;
    }
}
