// ReSharper disable CheckNamespace
namespace Matrix.Hashing.Benchmarks;

public partial class XxHash64Large
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.K4osHashXxHash)]
    public ulong K4osHashXxHash()
    {
        var result = global::K4os.Hash.xxHash.XXH64.DigestOf(_large);
        HashChecks.XxHash64Large(LibraryCatalog.K4osHashXxHash, result);
        return result;
    }
}
