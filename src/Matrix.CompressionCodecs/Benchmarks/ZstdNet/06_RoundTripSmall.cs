using System.Diagnostics.CodeAnalysis;
using ZstdNet;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming

namespace Matrix.CompressionCodecs.Benchmarks;

public partial class RoundTripSmall
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.ZstdNet)]
    [SuppressMessage("Performance", "CA1822:Mark members as static")]
    public byte[] ZstdNet()
    {
        using var encoder = new Compressor(new CompressionOptions(CompressionLevels.ZstdFastest));
        var compressed = encoder.Wrap(CompressionData.Small);

        using var decoder = new Decompressor();
        var restored = decoder.Unwrap(
            compressed,
            CompressionPayloads.MaximumDecompressedLength(CompressionData.SmallLength));

        CompressionChecks.Decompressed(LibraryCatalog.ZstdNet, restored, CompressionData.Small);
        return restored;
    }
}
