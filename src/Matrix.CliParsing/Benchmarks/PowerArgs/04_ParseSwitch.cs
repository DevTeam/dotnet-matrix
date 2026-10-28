// ReSharper disable CheckNamespace
namespace Matrix.CliParsing.Benchmarks;

public partial class ParseSwitch
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.PowerArgs)]
    public SwitchArguments PowerArgs()
    {
        var on = PowerArgsConfiguration.ReadSwitch(_withSwitch);
        CliChecks.RequireSwitchOn(LibraryCatalog.PowerArgs, on);
        var off = PowerArgsConfiguration.ReadSwitch(_withoutSwitch);
        CliChecks.RequireSwitchOff(LibraryCatalog.PowerArgs, off);
        return on;
    }
}
