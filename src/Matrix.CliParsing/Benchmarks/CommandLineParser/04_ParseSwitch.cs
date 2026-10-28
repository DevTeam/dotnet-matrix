// ReSharper disable CheckNamespace
namespace Matrix.CliParsing.Benchmarks;

public partial class ParseSwitch
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.CommandLineParser)]
    public SwitchArguments CommandLineParser()
    {
        var on = CommandLineParserConfiguration.ReadSwitch(_withSwitch);
        CliChecks.RequireSwitchOn(LibraryCatalog.CommandLineParser, on);
        var off = CommandLineParserConfiguration.ReadSwitch(_withoutSwitch);
        CliChecks.RequireSwitchOff(LibraryCatalog.CommandLineParser, off);
        return on;
    }
}
