// ReSharper disable CheckNamespace
namespace Matrix.CliParsing.Benchmarks;

public partial class ParseSwitch
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.CliFx)]
    public SwitchArguments CliFx()
    {
        var on = CliFxConfiguration.ReadSwitch(_withSwitch);
        CliChecks.RequireSwitchOn(LibraryCatalog.CliFx, on);
        var off = CliFxConfiguration.ReadSwitch(_withoutSwitch);
        CliChecks.RequireSwitchOff(LibraryCatalog.CliFx, off);
        return on;
    }
}
