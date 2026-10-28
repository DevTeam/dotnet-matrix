using System.Diagnostics.CodeAnalysis;
using ZstdNet;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming

namespace Matrix.CompressionCodecs.Benchmarks;

public partial class DecompressIncompressible
{
    private byte[] _ZstdNetPayload = null!;

    [GlobalSetup(Target = nameof(ZstdNet))]
    public void SetupZstdNet() =>
        _ZstdNetPayload = CompressionPayloads.CreateZstdNet(CompressionData.Incompressible);

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.ZstdNet)]
    [SuppressMessage("Performance", "CA1822:Mark members as static")]
    public byte[] ZstdNet()
    {
        using var decoder = new Decompressor();
        var restored = decoder.Unwrap(
            _ZstdNetPayload,
            CompressionPayloads.MaximumDecompressedLength(CompressionData.Incompressible.Length));

        CompressionChecks.Decompressed(LibraryCatalog.ZstdNet, restored, CompressionData.Incompressible);
        return restored;
    }
}
