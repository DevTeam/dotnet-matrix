using BenchmarkDotNet.Configs;

[assembly: BenchmarkDotNet.Attributes.Config(typeof(Matrix.CompressionCodecs.CompressionCodecsBenchmarkConfig))]

namespace Matrix.CompressionCodecs;

public sealed class CompressionCodecsBenchmarkConfig : ManualConfig
{
    public CompressionCodecsBenchmarkConfig()
    {
        // LZMA-SDK 22.1.1 ships SevenZip.dll without the DebuggableAttribute that
        // marks an optimized build, so BenchmarkDotNet rejects the module before
        // running anything. Benchmark the published package as-is; this exception
        // applies only to the Compression Codecs module.
        Options |= ConfigOptions.DisableOptimizationsValidator;
    }
}
