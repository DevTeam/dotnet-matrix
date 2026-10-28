using Spectre.Console;
using Spectre.Console.Cli;

namespace Matrix.CliParsing.Benchmarks;

internal sealed class SpectreReportSettings : CommandSettings
{
    [CommandOption("--input <PATH>")]
    public string Input { get; set; } = string.Empty;

    [CommandOption("--format <NAME>")]
    public string Format { get; set; } = string.Empty;

    [CommandOption("--verbose")]
    public bool Verbose { get; set; }

    [CommandOption("--tag <VALUE>")]
    public string[] Tags { get; set; } = [];

    [CommandArgument(0, "<OUTPUT>")]
    public string Output { get; set; } = string.Empty;

    public override ValidationResult Validate() =>
        string.IsNullOrEmpty(Input)
            ? ValidationResult.Error("The --input option is required.")
            : ValidationResult.Success();
}
