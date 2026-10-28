// ReSharper disable CheckNamespace
namespace Matrix.Hashing.Benchmarks;

public partial class XxHash64
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.StandartHashXxHash)]
    public ulong StandartHashXxHash()
    {
        var result = global::Standart.Hash.xxHash.xxHash64.ComputeHash(_small, _small.Length, 0UL);
        HashChecks.XxHash64(LibraryCatalog.StandartHashXxHash, result);
        return result;
    }
}
