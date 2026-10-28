using System.Diagnostics.CodeAnalysis;
using ZstdSharp;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming

namespace Matrix.CompressionCodecs.Benchmarks;

public partial class RoundTripSmall
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.ZstdSharp)]
    [SuppressMessage("Performance", "CA1822:Mark members as static")]
    public byte[] ZstdSharp()
    {
        using var encoder = new Compressor(CompressionLevels.ZstdFastest);
        var target = new byte[Compressor.GetCompressBound(CompressionData.Small.Length)];
        var size = encoder.Wrap(CompressionData.Small, target);
        var compressed = target[..size];

        using var decoder = new Decompressor();
        var restored = new byte[CompressionPayloads.MaximumDecompressedLength(CompressionData.SmallLength)];
        var decodedSize = decoder.Unwrap(compressed, restored);
        restored = restored[..decodedSize];

        CompressionChecks.Decompressed(LibraryCatalog.ZstdSharp, restored, CompressionData.Small);
        return restored;
    }
}
