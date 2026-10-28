// ReSharper disable CheckNamespace
namespace Matrix.Hashing.Benchmarks;

public partial class Crc32
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Crc32NET)]
    public uint Crc32NET()
    {
        var result = global::Force.Crc32.Crc32Algorithm.Compute(_small);
        HashChecks.Crc32(LibraryCatalog.Crc32NET, result);
        return result;
    }
}
