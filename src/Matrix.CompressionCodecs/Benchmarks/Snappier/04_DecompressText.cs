using System.Diagnostics.CodeAnalysis;
using Snappier;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming

namespace Matrix.CompressionCodecs.Benchmarks;

public partial class DecompressText
{
    private byte[] _SnappierPayload = null!;

    [GlobalSetup(Target = nameof(Snappier))]
    public void SetupSnappier() =>
        _SnappierPayload = CompressionPayloads.CreateSnappy(CompressionData.Compressible);

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Snappier)]
    [SuppressMessage("Performance", "CA1822:Mark members as static")]
    public byte[] Snappier()
    {
        var restored = Snappy.DecompressToArray(_SnappierPayload);

        CompressionChecks.Decompressed(LibraryCatalog.Snappier, restored, CompressionData.Compressible);
        return restored;
    }
}
