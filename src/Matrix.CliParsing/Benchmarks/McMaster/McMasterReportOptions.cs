using System.ComponentModel.DataAnnotations;
using McMaster.Extensions.CommandLineUtils;

namespace Matrix.CliParsing.Benchmarks;

internal sealed class McMasterReportOptions
{
    [Option("--input", ValueName = "path")]
    [Required]
    public string Input { get; set; } = string.Empty;

    [Option("--format", ValueName = "name")]
    public string Format { get; set; } = string.Empty;

    [Option("--verbose")]
    public bool Verbose { get; set; }

    [Option("--tag", CommandOptionType.MultipleValue, ValueName = "value")]
    public string[] Tags { get; set; } = [];

    [Argument(0, Name = "output")]
    [Required]
    public string Output { get; set; } = string.Empty;
}
