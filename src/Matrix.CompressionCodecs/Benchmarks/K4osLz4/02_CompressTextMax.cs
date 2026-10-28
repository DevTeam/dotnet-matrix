using System.Diagnostics.CodeAnalysis;
using K4os.Compression.LZ4;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming

namespace Matrix.CompressionCodecs.Benchmarks;

public partial class CompressTextMax
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.K4osLz4)]
    [SuppressMessage("Performance", "CA1822:Mark members as static")]
    public byte[] K4osLz4()
    {
        var target = new byte[LZ4Codec.MaximumOutputSize(CompressionData.Compressible.Length)];
        var size = LZ4Codec.Encode(CompressionData.Compressible, target, LZ4Level.L12_MAX);
        var compressed = target[..size];

        CompressionChecks.Compressed(LibraryCatalog.K4osLz4, compressed, CompressionData.Compressible.Length);
        CompressionChecks.CompressibleIsSmaller(LibraryCatalog.K4osLz4, compressed);
        return compressed;
    }
}
