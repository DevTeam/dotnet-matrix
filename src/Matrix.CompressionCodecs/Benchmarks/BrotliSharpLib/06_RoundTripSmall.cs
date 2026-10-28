using System.Diagnostics.CodeAnalysis;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming

namespace Matrix.CompressionCodecs.Benchmarks;

public partial class RoundTripSmall
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.BrotliSharpLib)]
    [SuppressMessage("Performance", "CA1822:Mark members as static")]
    public byte[] BrotliSharpLib()
    {
        var compressed = global::BrotliSharpLib.Brotli.CompressBuffer(
            CompressionData.Small,
            0,
            CompressionData.Small.Length,
            CompressionLevels.BrotliFastest,
            22,
            null!);

        var restored = global::BrotliSharpLib.Brotli.DecompressBuffer(
            compressed,
            0,
            compressed.Length,
            null!);

        CompressionChecks.Decompressed(LibraryCatalog.BrotliSharpLib, restored, CompressionData.Small);
        return restored;
    }
}
