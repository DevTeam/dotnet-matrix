// ReSharper disable CheckNamespace
namespace Matrix.Hashing.Benchmarks;

public partial class Crc32
{
    [Benchmark(Baseline = true)]
    [LibraryBenchmark(LibraryCatalog.SystemIOHashing)]
    public uint SystemIOHashing()
    {
        var result = global::System.IO.Hashing.Crc32.HashToUInt32(_small);
        HashChecks.Crc32(LibraryCatalog.SystemIOHashing, result);
        return result;
    }
}
