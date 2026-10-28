// ReSharper disable CheckNamespace
namespace Matrix.Hashing.Benchmarks;

public partial class XxHash3
{
    [Benchmark(Baseline = true)]
    [LibraryBenchmark(LibraryCatalog.SystemIOHashing)]
    public ulong SystemIOHashing()
    {
        var result = global::System.IO.Hashing.XxHash3.HashToUInt64(_small);
        HashChecks.XxHash3(LibraryCatalog.SystemIOHashing, result);
        return result;
    }
}