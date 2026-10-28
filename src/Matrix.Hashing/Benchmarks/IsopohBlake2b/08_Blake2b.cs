// ReSharper disable CheckNamespace
namespace Matrix.Hashing.Benchmarks;

public partial class Blake2b
{
    [Benchmark(Baseline = true)]
    [LibraryBenchmark(LibraryCatalog.IsopohBlake2b)]
    public ulong IsopohBlake2b()
    {
        var digest = global::Isopoh.Cryptography.Blake2b.Blake2B.ComputeHash(_small, _config, _secureArrayCall);
        var result = System.Buffers.Binary.BinaryPrimitives.ReadUInt64BigEndian(digest);
        HashChecks.Blake2b(LibraryCatalog.IsopohBlake2b, result);
        return result;
    }
}
