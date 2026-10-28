// ReSharper disable CheckNamespace
namespace Matrix.CliParsing.Benchmarks;

public partial class ParsePositionalArgument
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.PowerArgs)]
    public CompactArguments PowerArgs()
    {
        var result = PowerArgsConfiguration.ReadCompact(_args);
        CliChecks.RequireCompact(LibraryCatalog.PowerArgs, result);
        return result;
    }
}
