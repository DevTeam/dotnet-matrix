using System.Diagnostics.CodeAnalysis;
using SevenZip.Compression.LZMA;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming

namespace Matrix.CompressionCodecs.Benchmarks;

public partial class DecompressText
{
    private byte[] _LzmaSdkPayload = null!;
    private byte[] _LzmaSdkProperties = null!;

    [GlobalSetup(Target = nameof(LzmaSdk))]
    public void SetupLzmaSdk()
    {
        _LzmaSdkPayload = CompressionPayloads.CreateLzmaSdk(CompressionData.Compressible);
        _LzmaSdkProperties = _LzmaSdkPayload[..CompressionPayloads.LzmaSdkPropertiesLength];
    }

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.LzmaSdk)]
    [SuppressMessage("Performance", "CA1822:Mark members as static")]
    public byte[] LzmaSdk()
    {
        var decoder = new Decoder();
        decoder.SetDecoderProperties(_LzmaSdkProperties);

        using var output = new MemoryStream(CompressionData.Compressible.Length);
        using (var input = new MemoryStream(_LzmaSdkPayload, false))
        {
            input.Position = CompressionPayloads.LzmaSdkPropertiesLength;
            decoder.Code(
                input,
                output,
                _LzmaSdkPayload.Length - CompressionPayloads.LzmaSdkPropertiesLength,
                CompressionData.Compressible.Length,
                null!);
        }

        var restored = output.ToArray();

        CompressionChecks.Decompressed(LibraryCatalog.LzmaSdk, restored, CompressionData.Compressible);
        return restored;
    }
}
