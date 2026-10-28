// ReSharper disable CheckNamespace
namespace Matrix.CliParsing.Benchmarks;

public partial class ErrorMissingRequiredArgument
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.PowerArgs)]
    public ParseOutcome PowerArgs()
    {
        var result = PowerArgsConfiguration.ReadFailure(() => PowerArgsConfiguration.ReadCompact(_args));
        CliChecks.RequireFailure(LibraryCatalog.PowerArgs, result);
        return result;
    }
}
