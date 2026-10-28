// ReSharper disable CheckNamespace
namespace Matrix.Hashing.Benchmarks;

public partial class XxHash64Large
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.HashDepot)]
    public ulong HashDepot()
    {
        var result = global::HashDepot.XXHash.Hash64(_large, 0UL);
        HashChecks.XxHash64Large(LibraryCatalog.HashDepot, result);
        return result;
    }
}
