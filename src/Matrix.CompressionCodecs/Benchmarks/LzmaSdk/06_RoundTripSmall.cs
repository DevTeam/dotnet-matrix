using System.Diagnostics.CodeAnalysis;
using SevenZip.Compression.LZMA;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming

namespace Matrix.CompressionCodecs.Benchmarks;

public partial class RoundTripSmall
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.LzmaSdk)]
    [SuppressMessage("Performance", "CA1822:Mark members as static")]
    public byte[] LzmaSdk()
    {
        var encoder = new Encoder();
        encoder.SetCoderProperties(LzmaSdkProperties.EncoderPropertyIds, LzmaSdkProperties.Fastest);

        using var compressedBuffer = new MemoryStream();
        encoder.WriteCoderProperties(compressedBuffer);
        using (var input = new MemoryStream(CompressionData.Small, false))
        {
            encoder.Code(input, compressedBuffer, CompressionData.Small.Length, -1, null!);
        }

        var compressed = compressedBuffer.ToArray();
        var properties = compressed[..CompressionPayloads.LzmaSdkPropertiesLength];

        var decoder = new Decoder();
        decoder.SetDecoderProperties(properties);

        using var output = new MemoryStream(CompressionData.SmallLength);
        using (var input = new MemoryStream(compressed, false))
        {
            input.Position = properties.Length;
            decoder.Code(input, output, compressed.Length - properties.Length, CompressionData.SmallLength, null!);
        }

        var restored = output.ToArray();

        CompressionChecks.Decompressed(LibraryCatalog.LzmaSdk, restored, CompressionData.Small);
        return restored;
    }
}
