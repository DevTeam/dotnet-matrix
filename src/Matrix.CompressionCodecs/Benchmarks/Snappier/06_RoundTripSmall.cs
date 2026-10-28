using System.Diagnostics.CodeAnalysis;
using Snappier;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming

namespace Matrix.CompressionCodecs.Benchmarks;

public partial class RoundTripSmall
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Snappier)]
    [SuppressMessage("Performance", "CA1822:Mark members as static")]
    public byte[] Snappier()
    {
        var compressed = Snappy.CompressToArray(CompressionData.Small);

        var restored = Snappy.DecompressToArray(compressed);

        CompressionChecks.Decompressed(LibraryCatalog.Snappier, restored, CompressionData.Small);
        return restored;
    }
}
