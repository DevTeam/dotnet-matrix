using System.Diagnostics.CodeAnalysis;
using SevenZip.Compression.LZMA;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming

namespace Matrix.CompressionCodecs.Benchmarks;

public partial class CompressIncompressible
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.LzmaSdk)]
    [SuppressMessage("Performance", "CA1822:Mark members as static")]
    public byte[] LzmaSdk()
    {
        var encoder = new Encoder();
        encoder.SetCoderProperties(LzmaSdkProperties.EncoderPropertyIds, LzmaSdkProperties.Fastest);

        using var destination = new MemoryStream();
        encoder.WriteCoderProperties(destination);
        using (var input = new MemoryStream(CompressionData.Incompressible, false))
        {
            encoder.Code(input, destination, CompressionData.Incompressible.Length, -1, null!);
        }

        var compressed = destination.ToArray();

        CompressionChecks.Compressed(LibraryCatalog.LzmaSdk, compressed, CompressionData.Incompressible.Length);
        return compressed;
    }
}
