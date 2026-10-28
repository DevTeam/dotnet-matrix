using System.Diagnostics.CodeAnalysis;
using ZstdSharp;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming

namespace Matrix.CompressionCodecs.Benchmarks;

public partial class CompressIncompressible
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.ZstdSharp)]
    [SuppressMessage("Performance", "CA1822:Mark members as static")]
    public byte[] ZstdSharp()
    {
        using var encoder = new Compressor(CompressionLevels.ZstdFastest);
        var target = new byte[Compressor.GetCompressBound(CompressionData.Incompressible.Length)];
        var size = encoder.Wrap(CompressionData.Incompressible, target);
        var compressed = target[..size];

        CompressionChecks.Compressed(LibraryCatalog.ZstdSharp, compressed, CompressionData.Incompressible.Length);
        return compressed;
    }
}
