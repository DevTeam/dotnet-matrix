// ReSharper disable CheckNamespace
namespace Matrix.Hashing.Benchmarks;

public partial class XxHash32
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.StandartHashXxHash)]
    public uint StandartHashXxHash()
    {
        var result = global::Standart.Hash.xxHash.xxHash32.ComputeHash(_small, _small.Length, 0U);
        HashChecks.XxHash32(LibraryCatalog.StandartHashXxHash, result);
        return result;
    }
}