// ReSharper disable CheckNamespace
namespace Matrix.CliParsing.Benchmarks;

public partial class ParseOptionValue
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.PowerArgs)]
    public ReportArguments PowerArgs()
    {
        var result = PowerArgsConfiguration.ReadReport(_args);
        CliChecks.RequireReport(LibraryCatalog.PowerArgs, result);
        return result;
    }
}
