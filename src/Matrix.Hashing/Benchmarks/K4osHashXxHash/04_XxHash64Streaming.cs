// ReSharper disable CheckNamespace
namespace Matrix.Hashing.Benchmarks;

public partial class XxHash64Streaming
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.K4osHashXxHash)]
    public ulong K4osHashXxHash()
    {
        var hasher = new global::K4os.Hash.xxHash.XXH64();
        for (var offset = 0; offset < _small.Length; offset += HashData.StreamingChunkLength)
        {
            hasher.Update(_small.AsSpan(offset, HashData.StreamingChunkLength));
        }

        var result = hasher.Digest();
        HashChecks.XxHash64Streaming(LibraryCatalog.K4osHashXxHash, result);
        return result;
    }
}