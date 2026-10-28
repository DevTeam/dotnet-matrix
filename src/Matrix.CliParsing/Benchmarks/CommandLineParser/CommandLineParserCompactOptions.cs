using CommandLine;

namespace Matrix.CliParsing.Benchmarks;

internal sealed class CommandLineParserCompactOptions
{
    [Option("level", Required = true, HelpText = "Compression level")]
    public int Level { get; set; }

    [Value(0, Required = true, HelpText = "Input path")]
    public string Input { get; set; } = string.Empty;
}
