// ReSharper disable CheckNamespace
namespace Matrix.Hashing.Benchmarks;

public partial class XxHash64Streaming
{
    [Benchmark(Baseline = true)]
    [LibraryBenchmark(LibraryCatalog.SystemIOHashing)]
    public ulong SystemIOHashing()
    {
        var hasher = new global::System.IO.Hashing.XxHash64();
        for (var offset = 0; offset < _small.Length; offset += HashData.StreamingChunkLength)
        {
            hasher.Append(_small.AsSpan(offset, HashData.StreamingChunkLength));
        }

        var result = hasher.GetCurrentHashAsUInt64();
        HashChecks.XxHash64Streaming(LibraryCatalog.SystemIOHashing, result);
        return result;
    }
}