using CommandDotNet;
// ReSharper disable CheckNamespace
namespace Matrix.CliParsing.Benchmarks;

public partial class BindTypedModel
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.CommandDotNet)]
    public CompactArguments CommandDotNet()
    {
        var result = CommandDotNetConfiguration.ReadCompactModel(_args);
        CliChecks.RequireCompact(LibraryCatalog.CommandDotNet, result);
        return result;
    }
}
