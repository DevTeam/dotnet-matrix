using CommandLine;

namespace Matrix.CliParsing.Benchmarks;

internal sealed class CommandLineParserReportOptions
{
    [Option("input", Required = true, HelpText = "Input path")]
    public string Input { get; set; } = string.Empty;

    [Option("format", HelpText = "Output format")]
    public string Format { get; set; } = string.Empty;

    [Option("verbose", HelpText = "Verbose output")]
    public bool Verbose { get; set; }

    [Option("tag", HelpText = "Tag value")]
    public IEnumerable<string> Tags { get; set; } = [];

    [Value(0, Required = true, HelpText = "Output path")]
    public string Output { get; set; } = string.Empty;
}
