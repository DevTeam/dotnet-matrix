using System.Diagnostics.CodeAnalysis;
using Snappier;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming

namespace Matrix.CompressionCodecs.Benchmarks;

public partial class CompressIncompressible
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Snappier)]
    [SuppressMessage("Performance", "CA1822:Mark members as static")]
    public byte[] Snappier()
    {
        var compressed = Snappy.CompressToArray(CompressionData.Incompressible);

        CompressionChecks.Compressed(LibraryCatalog.Snappier, compressed, CompressionData.Incompressible.Length);
        return compressed;
    }
}
