// ReSharper disable CheckNamespace
namespace Matrix.Hashing.Benchmarks;

public partial class XxHash64
{
    [Benchmark(Baseline = true)]
    [LibraryBenchmark(LibraryCatalog.SystemIOHashing)]
    public ulong SystemIOHashing()
    {
        var result = global::System.IO.Hashing.XxHash64.HashToUInt64(_small);
        HashChecks.XxHash64(LibraryCatalog.SystemIOHashing, result);
        return result;
    }
}
