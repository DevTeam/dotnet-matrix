using System.Diagnostics.CodeAnalysis;
using System.IO.Compression;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming

namespace Matrix.CompressionCodecs.Benchmarks;

public partial class CompressIncompressible
{
    [Benchmark(Baseline = true)]
    [LibraryBenchmark(LibraryCatalog.SystemIOCompression)]
    [SuppressMessage("Performance", "CA1822:Mark members as static")]
    public byte[] SystemIOCompression()
    {
        using var destination = new MemoryStream();
        using (var encoder = new BrotliStream(destination, CompressionLevel.Fastest, true))
        {
            encoder.Write(CompressionData.Incompressible);
        }

        var compressed = destination.ToArray();

        CompressionChecks.Compressed(LibraryCatalog.SystemIOCompression, compressed, CompressionData.Incompressible.Length);
        return compressed;
    }
}
