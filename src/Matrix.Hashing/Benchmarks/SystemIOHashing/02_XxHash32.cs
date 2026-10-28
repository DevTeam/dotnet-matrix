// ReSharper disable CheckNamespace
namespace Matrix.Hashing.Benchmarks;

public partial class XxHash32
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.SystemIOHashing)]
    public uint SystemIOHashing()
    {
        var result = global::System.IO.Hashing.XxHash32.HashToUInt32(_small);
        HashChecks.XxHash32(LibraryCatalog.SystemIOHashing, result);
        return result;
    }
}