using System.Diagnostics.CodeAnalysis;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming

namespace Matrix.CompressionCodecs.Benchmarks;

public partial class CompressTextMax
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.BrotliSharpLib)]
    [SuppressMessage("Performance", "CA1822:Mark members as static")]
    public byte[] BrotliSharpLib()
    {
        var compressed = global::BrotliSharpLib.Brotli.CompressBuffer(
            CompressionData.Compressible,
            0,
            CompressionData.Compressible.Length,
            CompressionLevels.BrotliMaximum,
            22,
            null!);

        CompressionChecks.Compressed(LibraryCatalog.BrotliSharpLib, compressed, CompressionData.Compressible.Length);
        CompressionChecks.CompressibleIsSmaller(LibraryCatalog.BrotliSharpLib, compressed);
        return compressed;
    }
}
