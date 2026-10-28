// ReSharper disable CheckNamespace
namespace Matrix.Hashing.Benchmarks;

public partial class XxHash32
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.K4osHashXxHash)]
    public uint K4osHashXxHash()
    {
        var result = global::K4os.Hash.xxHash.XXH32.DigestOf(_small);
        HashChecks.XxHash32(LibraryCatalog.K4osHashXxHash, result);
        return result;
    }
}