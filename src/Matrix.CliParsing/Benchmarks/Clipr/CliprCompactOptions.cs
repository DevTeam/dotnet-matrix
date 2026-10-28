using clipr;

namespace Matrix.CliParsing.Benchmarks;

internal sealed class CliprCompactOptions
{
    [NamedArgument("level", Action = ParseAction.Store, Required = true)]
    public int Level { get; set; }

    [PositionalArgument(0)]
    public string Input { get; set; } = string.Empty;
}
