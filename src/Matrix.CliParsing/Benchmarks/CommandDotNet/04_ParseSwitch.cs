using CommandDotNet;
// ReSharper disable CheckNamespace
namespace Matrix.CliParsing.Benchmarks;

public partial class ParseSwitch
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.CommandDotNet)]
    public SwitchArguments CommandDotNet()
    {
        var on = CommandDotNetConfiguration.ReadSwitch(_withSwitch);
        CliChecks.RequireSwitchOn(LibraryCatalog.CommandDotNet, on);
        var off = CommandDotNetConfiguration.ReadSwitch(_withoutSwitch);
        CliChecks.RequireSwitchOff(LibraryCatalog.CommandDotNet, off);
        return on;
    }
}
