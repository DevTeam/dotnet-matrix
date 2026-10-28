using McMaster.Extensions.CommandLineUtils;
// ReSharper disable CheckNamespace
namespace Matrix.CliParsing.Benchmarks;

public partial class ParseRepeatedOption
{
    private readonly CommandLineApplication<McMasterReportOptions> _mcApplication = McMasterConfiguration.Create<McMasterReportOptions>();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.McMasterCommandLineUtils)]
    public TagArguments McMasterCommandLineUtils()
    {
        var result = McMasterConfiguration.ReadTags(_mcApplication, _args);
        CliChecks.RequireTags(LibraryCatalog.McMasterCommandLineUtils, result);
        return result;
    }
}
