using Spectre.Console.Cli;

namespace Matrix.CliParsing.Benchmarks;

internal sealed class SpectreCompactSettings : CommandSettings
{
    [CommandOption("--level <N>")]
    public int Level { get; set; }

    [CommandArgument(0, "<INPUT>")]
    public string Input { get; set; } = string.Empty;
}
