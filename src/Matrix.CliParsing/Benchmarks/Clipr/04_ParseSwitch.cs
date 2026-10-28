// ReSharper disable CheckNamespace
namespace Matrix.CliParsing.Benchmarks;

public partial class ParseSwitch
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Clipr)]
    public SwitchArguments Clipr()
    {
        var on = CliprConfiguration.ReadSwitch(_withSwitch);
        CliChecks.RequireSwitchOn(LibraryCatalog.Clipr, on);
        var off = CliprConfiguration.ReadSwitch(_withoutSwitch);
        CliChecks.RequireSwitchOff(LibraryCatalog.Clipr, off);
        return on;
    }
}
