using System.CommandLine;
// ReSharper disable CheckNamespace
namespace Matrix.CliParsing.Benchmarks;

public partial class ParseSwitch
{
    private readonly RootCommand _scCommand = ScCommandConfiguration.Create();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.SystemCommandLine)]
    public SwitchArguments SystemCommandLine()
    {
        var on = ScCommandConfiguration.ReadSwitch(_scCommand, _withSwitch);
        CliChecks.RequireSwitchOn(LibraryCatalog.SystemCommandLine, on);
        var off = ScCommandConfiguration.ReadSwitch(_scCommand, _withoutSwitch);
        CliChecks.RequireSwitchOff(LibraryCatalog.SystemCommandLine, off);
        return on;
    }
}
