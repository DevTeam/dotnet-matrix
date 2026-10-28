using McMaster.Extensions.CommandLineUtils;
// ReSharper disable CheckNamespace
namespace Matrix.CliParsing.Benchmarks;

public partial class ParseSwitch
{
    // One application instance per command line: a reused instance keeps the model it
    // bound first, so the switch state of the previous parse would leak into the next.
    private readonly CommandLineApplication<McMasterReportOptions> _on = McMasterConfiguration.Create<McMasterReportOptions>();
    private readonly CommandLineApplication<McMasterReportOptions> _off = McMasterConfiguration.Create<McMasterReportOptions>();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.McMasterCommandLineUtils)]
    public SwitchArguments McMasterCommandLineUtils()
    {
        var on = McMasterConfiguration.ReadSwitch(_on, _withSwitch);
        CliChecks.RequireSwitchOn(LibraryCatalog.McMasterCommandLineUtils, on);
        var off = McMasterConfiguration.ReadSwitch(_off, _withoutSwitch);
        CliChecks.RequireSwitchOff(LibraryCatalog.McMasterCommandLineUtils, off);
        return on;
    }
}
