using System.Diagnostics.CodeAnalysis;
using K4os.Compression.LZ4;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming

namespace Matrix.CompressionCodecs.Benchmarks;

public partial class RoundTripSmall
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.K4osLz4)]
    [SuppressMessage("Performance", "CA1822:Mark members as static")]
    public byte[] K4osLz4()
    {
        var target = new byte[LZ4Codec.MaximumOutputSize(CompressionData.Small.Length)];
        var size = LZ4Codec.Encode(CompressionData.Small, target, LZ4Level.L00_FAST);
        var compressed = target[..size];

        var restored = new byte[CompressionData.SmallLength];
        LZ4Codec.Decode(compressed, restored);

        CompressionChecks.Decompressed(LibraryCatalog.K4osLz4, restored, CompressionData.Small);
        return restored;
    }
}
