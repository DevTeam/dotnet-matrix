// ReSharper disable CheckNamespace
namespace Matrix.Hashing.Benchmarks;

public partial class FarmHash64
{
    [Benchmark(Baseline = true)]
    [LibraryBenchmark(LibraryCatalog.FarmHashSharp)]
    public ulong FarmHashSharp()
    {
        var result = global::Farmhash.Sharp.Farmhash.Hash64(_small);
        HashChecks.FarmHash64(LibraryCatalog.FarmHashSharp, result);
        return result;
    }
}
