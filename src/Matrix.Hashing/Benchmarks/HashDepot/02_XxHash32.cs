// ReSharper disable CheckNamespace
namespace Matrix.Hashing.Benchmarks;

public partial class XxHash32
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.HashDepot)]
    public uint HashDepot()
    {
        var result = global::HashDepot.XXHash.Hash32(_small, 0U);
        HashChecks.XxHash32(LibraryCatalog.HashDepot, result);
        return result;
    }
}