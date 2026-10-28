// ReSharper disable CheckNamespace
namespace Matrix.Hashing.Benchmarks;

public partial class XxHash64Large
{
    [Benchmark(Baseline = true)]
    [LibraryBenchmark(LibraryCatalog.SystemIOHashing)]
    public ulong SystemIOHashing()
    {
        var result = global::System.IO.Hashing.XxHash64.HashToUInt64(_large);
        HashChecks.XxHash64Large(LibraryCatalog.SystemIOHashing, result);
        return result;
    }
}
