using System.Diagnostics.CodeAnalysis;
using ZstdSharp;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming

namespace Matrix.CompressionCodecs.Benchmarks;

public partial class DecompressText
{
    private byte[] _ZstdSharpPayload = null!;

    [GlobalSetup(Target = nameof(ZstdSharp))]
    public void SetupZstdSharp() =>
        _ZstdSharpPayload = CompressionPayloads.CreateZstdSharp(CompressionData.Compressible);

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.ZstdSharp)]
    [SuppressMessage("Performance", "CA1822:Mark members as static")]
    public byte[] ZstdSharp()
    {
        using var decoder = new Decompressor();
        var restored = new byte[CompressionPayloads.MaximumDecompressedLength(CompressionData.Compressible.Length)];
        var decodedSize = decoder.Unwrap(_ZstdSharpPayload, restored);
        restored = restored[..decodedSize];

        CompressionChecks.Decompressed(LibraryCatalog.ZstdSharp, restored, CompressionData.Compressible);
        return restored;
    }
}
