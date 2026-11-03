using CommandDotNet;
// ReSharper disable CheckNamespace
namespace Matrix.CliParsing.Benchmarks;

public partial class ParsePositionalArgument
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.CommandDotNet)]
    public CompactArguments CommandDotNet()
    {
        var result = CommandDotNetConfiguration.ReadCompact(_args);
        CliChecks.RequireCompact(LibraryCatalog.CommandDotNet, result);
        return result;
    }
}
