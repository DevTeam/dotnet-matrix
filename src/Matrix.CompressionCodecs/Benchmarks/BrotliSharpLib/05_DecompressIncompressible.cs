using System.Diagnostics.CodeAnalysis;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming

namespace Matrix.CompressionCodecs.Benchmarks;

public partial class DecompressIncompressible
{
    private byte[] _BrotliSharpLibPayload = null!;

    [GlobalSetup(Target = nameof(BrotliSharpLib))]
    public void SetupBrotliSharpLib() =>
        _BrotliSharpLibPayload = CompressionPayloads.CreateBrotliSharp(CompressionData.Incompressible);

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.BrotliSharpLib)]
    [SuppressMessage("Performance", "CA1822:Mark members as static")]
    public byte[] BrotliSharpLib()
    {
        var restored = global::BrotliSharpLib.Brotli.DecompressBuffer(
            _BrotliSharpLibPayload,
            0,
            _BrotliSharpLibPayload.Length,
            null!);

        CompressionChecks.Decompressed(LibraryCatalog.BrotliSharpLib, restored, CompressionData.Incompressible);
        return restored;
    }
}
