// ReSharper disable CheckNamespace
namespace Matrix.Hashing.Benchmarks;

public partial class XxHash64Large
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.StandartHashXxHash)]
    public ulong StandartHashXxHash()
    {
        var result = global::Standart.Hash.xxHash.xxHash64.ComputeHash(_large, _large.Length, 0UL);
        HashChecks.XxHash64Large(LibraryCatalog.StandartHashXxHash, result);
        return result;
    }
}
