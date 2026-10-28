// ReSharper disable CheckNamespace
namespace Matrix.Hashing.Benchmarks;

public partial class Blake3Hash
{
    [Benchmark(Baseline = true)]
    [LibraryBenchmark(LibraryCatalog.Blake3)]
    public ulong Blake3()
    {
        var result = ReadDigestPrefix(global::Blake3.Hasher.Hash(_small).AsSpan());
        HashChecks.Blake3(LibraryCatalog.Blake3, result);
        return result;
    }

    private static ulong ReadDigestPrefix(ReadOnlySpan<byte> digest) =>
        System.Buffers.Binary.BinaryPrimitives.ReadUInt64BigEndian(digest);
}
