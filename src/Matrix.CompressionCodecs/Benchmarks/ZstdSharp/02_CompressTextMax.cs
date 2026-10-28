using System.Diagnostics.CodeAnalysis;
using ZstdSharp;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming

namespace Matrix.CompressionCodecs.Benchmarks;

public partial class CompressTextMax
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.ZstdSharp)]
    [SuppressMessage("Performance", "CA1822:Mark members as static")]
    public byte[] ZstdSharp()
    {
        using var encoder = new Compressor(CompressionLevels.ZstdMaximum);
        var target = new byte[Compressor.GetCompressBound(CompressionData.Compressible.Length)];
        var size = encoder.Wrap(CompressionData.Compressible, target);
        var compressed = target[..size];

        CompressionChecks.Compressed(LibraryCatalog.ZstdSharp, compressed, CompressionData.Compressible.Length);
        CompressionChecks.CompressibleIsSmaller(LibraryCatalog.ZstdSharp, compressed);
        return compressed;
    }
}
