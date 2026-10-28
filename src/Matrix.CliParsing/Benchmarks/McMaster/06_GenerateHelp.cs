using McMaster.Extensions.CommandLineUtils;
// ReSharper disable CheckNamespace
namespace Matrix.CliParsing.Benchmarks;

public partial class GenerateHelp
{
    private readonly CommandLineApplication<McMasterReportOptions> _mcApplication = McMasterConfiguration.Create<McMasterReportOptions>();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.McMasterCommandLineUtils)]
    public GeneratedHelp McMasterCommandLineUtils()
    {
        var result = McMasterConfiguration.RenderHelp(_mcApplication);
        CliChecks.RequireHelp(LibraryCatalog.McMasterCommandLineUtils, result);
        return result;
    }
}
