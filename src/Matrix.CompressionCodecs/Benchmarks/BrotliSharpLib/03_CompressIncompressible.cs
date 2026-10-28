using System.Diagnostics.CodeAnalysis;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming

namespace Matrix.CompressionCodecs.Benchmarks;

public partial class CompressIncompressible
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.BrotliSharpLib)]
    [SuppressMessage("Performance", "CA1822:Mark members as static")]
    public byte[] BrotliSharpLib()
    {
        var compressed = global::BrotliSharpLib.Brotli.CompressBuffer(
            CompressionData.Incompressible,
            0,
            CompressionData.Incompressible.Length,
            CompressionLevels.BrotliFastest,
            22,
            null!);

        CompressionChecks.Compressed(LibraryCatalog.BrotliSharpLib, compressed, CompressionData.Incompressible.Length);
        return compressed;
    }
}
