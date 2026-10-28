using System.ComponentModel.DataAnnotations;
using McMaster.Extensions.CommandLineUtils;

namespace Matrix.CliParsing.Benchmarks;

internal sealed class McMasterCompactOptions
{
    [Option("--level", ValueName = "n")]
    [Required]
    public int Level { get; set; }

    [Argument(0, Name = "input")]
    [Required]
    public string Input { get; set; } = string.Empty;
}
