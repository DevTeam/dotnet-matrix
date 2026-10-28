using System.Diagnostics.CodeAnalysis;
using ZstdNet;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming

namespace Matrix.CompressionCodecs.Benchmarks;

public partial class CompressText
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.ZstdNet)]
    [SuppressMessage("Performance", "CA1822:Mark members as static")]
    public byte[] ZstdNet()
    {
        using var encoder = new Compressor(new CompressionOptions(CompressionLevels.ZstdFastest));
        var compressed = encoder.Wrap(CompressionData.Compressible);

        CompressionChecks.Compressed(LibraryCatalog.ZstdNet, compressed, CompressionData.Compressible.Length);
        CompressionChecks.CompressibleIsSmaller(LibraryCatalog.ZstdNet, compressed);
        return compressed;
    }
}
