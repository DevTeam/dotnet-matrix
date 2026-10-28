// ReSharper disable CheckNamespace
namespace Matrix.Hashing.Benchmarks;

public partial class XxHash3
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.StandartHashXxHash)]
    public ulong StandartHashXxHash()
    {
        var result = global::Standart.Hash.xxHash.xxHash3.ComputeHash(_small, _small.Length, 0UL);
        HashChecks.XxHash3(LibraryCatalog.StandartHashXxHash, result);
        return result;
    }
}