using System.Diagnostics.CodeAnalysis;
using SevenZip.Compression.LZMA;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming

namespace Matrix.CompressionCodecs.Benchmarks;

public partial class DecompressIncompressible
{
    private byte[] _LzmaSdkPayload = null!;
    private byte[] _LzmaSdkProperties = null!;

    [GlobalSetup(Target = nameof(LzmaSdk))]
    public void SetupLzmaSdk()
    {
        _LzmaSdkPayload = CompressionPayloads.CreateLzmaSdk(CompressionData.Incompressible);
        _LzmaSdkProperties = _LzmaSdkPayload[..CompressionPayloads.LzmaSdkPropertiesLength];
    }

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.LzmaSdk)]
    [SuppressMessage("Performance", "CA1822:Mark members as static")]
    public byte[] LzmaSdk()
    {
        var decoder = new Decoder();
        decoder.SetDecoderProperties(_LzmaSdkProperties);

        using var output = new MemoryStream(CompressionData.Incompressible.Length);
        using (var input = new MemoryStream(_LzmaSdkPayload, false))
        {
            input.Position = CompressionPayloads.LzmaSdkPropertiesLength;
            decoder.Code(
                input,
                output,
                _LzmaSdkPayload.Length - CompressionPayloads.LzmaSdkPropertiesLength,
                CompressionData.Incompressible.Length,
                null!);
        }

        var restored = output.ToArray();

        CompressionChecks.Decompressed(LibraryCatalog.LzmaSdk, restored, CompressionData.Incompressible);
        return restored;
    }
}
