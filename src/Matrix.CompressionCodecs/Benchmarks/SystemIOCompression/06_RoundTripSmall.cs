using System.Diagnostics.CodeAnalysis;
using System.IO.Compression;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming

namespace Matrix.CompressionCodecs.Benchmarks;

public partial class RoundTripSmall
{
    [Benchmark(Baseline = true)]
    [LibraryBenchmark(LibraryCatalog.SystemIOCompression)]
    [SuppressMessage("Performance", "CA1822:Mark members as static")]
    public byte[] SystemIOCompression()
    {
        using var destination = new MemoryStream();
        using (var encoder = new BrotliStream(destination, CompressionLevel.Fastest, true))
        {
            encoder.Write(CompressionData.Small);
        }

        var compressed = destination.ToArray();

        using var source = new MemoryStream(compressed, false);
        using var decoder = new BrotliStream(source, CompressionMode.Decompress, false);
        using var restoreBuffer = new MemoryStream(CompressionPayloads.MaximumDecompressedLength(CompressionData.SmallLength));
        decoder.CopyTo(restoreBuffer);
        var restored = restoreBuffer.ToArray();

        CompressionChecks.Decompressed(LibraryCatalog.SystemIOCompression, restored, CompressionData.Small);
        return restored;
    }
}
