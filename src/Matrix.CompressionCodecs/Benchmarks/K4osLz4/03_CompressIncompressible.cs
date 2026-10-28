using System.Diagnostics.CodeAnalysis;
using K4os.Compression.LZ4;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming

namespace Matrix.CompressionCodecs.Benchmarks;

public partial class CompressIncompressible
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.K4osLz4)]
    [SuppressMessage("Performance", "CA1822:Mark members as static")]
    public byte[] K4osLz4()
    {
        var target = new byte[LZ4Codec.MaximumOutputSize(CompressionData.Incompressible.Length)];
        var size = LZ4Codec.Encode(CompressionData.Incompressible, target, LZ4Level.L00_FAST);
        var compressed = target[..size];

        CompressionChecks.Compressed(LibraryCatalog.K4osLz4, compressed, CompressionData.Incompressible.Length);
        return compressed;
    }
}
