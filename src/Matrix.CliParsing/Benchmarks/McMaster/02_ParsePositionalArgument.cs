using McMaster.Extensions.CommandLineUtils;
// ReSharper disable CheckNamespace
namespace Matrix.CliParsing.Benchmarks;

public partial class ParsePositionalArgument
{
    private readonly CommandLineApplication<McMasterCompactOptions> _mcApplication = McMasterConfiguration.Create<McMasterCompactOptions>();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.McMasterCommandLineUtils)]
    public CompactArguments McMasterCommandLineUtils()
    {
        var result = McMasterConfiguration.ReadCompact(_mcApplication, _args);
        CliChecks.RequireCompact(LibraryCatalog.McMasterCommandLineUtils, result);
        return result;
    }
}
