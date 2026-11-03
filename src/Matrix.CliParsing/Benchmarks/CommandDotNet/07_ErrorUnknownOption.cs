using CommandDotNet;
// ReSharper disable CheckNamespace
namespace Matrix.CliParsing.Benchmarks;

public partial class ErrorUnknownOption
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.CommandDotNet)]
    public ParseOutcome CommandDotNet()
    {
        var result = CommandDotNetConfiguration.ReadReportFailure(_args);
        CliChecks.RequireFailure(LibraryCatalog.CommandDotNet, result);
        return result;
    }
}
