using CommandDotNet;
// ReSharper disable CheckNamespace
namespace Matrix.CliParsing.Benchmarks;

public partial class ErrorInvalidType
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.CommandDotNet)]
    public ParseOutcome CommandDotNet()
    {
        var result = CommandDotNetConfiguration.ReadCompactFailure(_args);
        CliChecks.RequireFailure(LibraryCatalog.CommandDotNet, result);
        return result;
    }
}
