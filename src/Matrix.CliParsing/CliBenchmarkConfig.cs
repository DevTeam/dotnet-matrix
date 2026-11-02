using BenchmarkDotNet.Configs;

[assembly: BenchmarkDotNet.Attributes.Config(typeof(Matrix.CliParsing.CliBenchmarkConfig))]

namespace Matrix.CliParsing;

public sealed class CliBenchmarkConfig : ManualConfig
{
    public CliBenchmarkConfig()
    {
        // PowerArgs 4.0.3 ships a non-optimized assembly. Benchmark the published
        // package as-is; this exception applies only to the CLI Parsing module.
        Options |= ConfigOptions.DisableOptimizationsValidator;
    }
}
