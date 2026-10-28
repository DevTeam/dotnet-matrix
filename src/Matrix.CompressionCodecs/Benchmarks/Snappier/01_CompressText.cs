using System.Diagnostics.CodeAnalysis;
using Snappier;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming

namespace Matrix.CompressionCodecs.Benchmarks;

public partial class CompressText
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Snappier)]
    [SuppressMessage("Performance", "CA1822:Mark members as static")]
    public byte[] Snappier()
    {
        var compressed = Snappy.CompressToArray(CompressionData.Compressible);

        CompressionChecks.Compressed(LibraryCatalog.Snappier, compressed, CompressionData.Compressible.Length);
        CompressionChecks.CompressibleIsSmaller(LibraryCatalog.Snappier, compressed);
        return compressed;
    }
}
