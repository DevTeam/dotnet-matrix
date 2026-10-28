// ReSharper disable CheckNamespace
namespace Matrix.Hashing.Benchmarks;

public partial class XxHash64
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.HashDepot)]
    public ulong HashDepot()
    {
        var result = global::HashDepot.XXHash.Hash64(_small, 0UL);
        HashChecks.XxHash64(LibraryCatalog.HashDepot, result);
        return result;
    }
}
