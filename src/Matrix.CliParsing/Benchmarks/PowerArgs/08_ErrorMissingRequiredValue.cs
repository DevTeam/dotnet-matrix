// ReSharper disable CheckNamespace
namespace Matrix.CliParsing.Benchmarks;

public partial class ErrorMissingRequiredValue
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.PowerArgs)]
    public ParseOutcome PowerArgs()
    {
        var result = PowerArgsConfiguration.ReadFailure(() => PowerArgsConfiguration.ReadReport(_args));
        CliChecks.RequireFailure(LibraryCatalog.PowerArgs, result);
        return result;
    }
}
