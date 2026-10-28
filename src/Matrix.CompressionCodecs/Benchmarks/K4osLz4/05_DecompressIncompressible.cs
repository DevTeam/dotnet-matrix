using System.Diagnostics.CodeAnalysis;
using K4os.Compression.LZ4;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming

namespace Matrix.CompressionCodecs.Benchmarks;

public partial class DecompressIncompressible
{
    private byte[] _K4osLz4Payload = null!;

    [GlobalSetup(Target = nameof(K4osLz4))]
    public void SetupK4osLz4() =>
        _K4osLz4Payload = CompressionPayloads.CreateLz4(CompressionData.Incompressible);

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.K4osLz4)]
    [SuppressMessage("Performance", "CA1822:Mark members as static")]
    public byte[] K4osLz4()
    {
        var restored = new byte[CompressionData.Incompressible.Length];
        LZ4Codec.Decode(_K4osLz4Payload, restored);

        CompressionChecks.Decompressed(LibraryCatalog.K4osLz4, restored, CompressionData.Incompressible);
        return restored;
    }
}
