using System.Diagnostics.CodeAnalysis;
using System.IO.Compression;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming

namespace Matrix.CompressionCodecs.Benchmarks;

public partial class DecompressIncompressible
{
    private byte[] _SystemIOCompressionPayload = null!;

    [GlobalSetup(Target = nameof(SystemIOCompression))]
    public void SetupSystemIOCompression() =>
        _SystemIOCompressionPayload = CompressionPayloads.CreateBrotli(CompressionData.Incompressible);

    [Benchmark(Baseline = true)]
    [LibraryBenchmark(LibraryCatalog.SystemIOCompression)]
    [SuppressMessage("Performance", "CA1822:Mark members as static")]
    public byte[] SystemIOCompression()
    {
        using var source = new MemoryStream(_SystemIOCompressionPayload, false);
        using var decoder = new BrotliStream(source, CompressionMode.Decompress, false);
        using var restoreBuffer = new MemoryStream(CompressionPayloads.MaximumDecompressedLength(CompressionData.Incompressible.Length));
        decoder.CopyTo(restoreBuffer);
        var restored = restoreBuffer.ToArray();

        CompressionChecks.Decompressed(LibraryCatalog.SystemIOCompression, restored, CompressionData.Incompressible);
        return restored;
    }
}
