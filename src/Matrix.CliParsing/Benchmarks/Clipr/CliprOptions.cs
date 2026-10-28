using clipr;

namespace Matrix.CliParsing.Benchmarks;

internal sealed class CliprOptions
{
    [NamedArgument("input", Action = ParseAction.Store, Required = true)]
    public string Input { get; set; } = string.Empty;

    [NamedArgument("format", Action = ParseAction.Store)]
    public string Format { get; set; } = string.Empty;

    [NamedArgument("verbose", Action = ParseAction.StoreTrue)]
    public bool Verbose { get; set; }

    [NamedArgument("tag", Action = ParseAction.Append)]
    public List<string> Tags { get; set; } = [];

    [PositionalArgument(0)]
    public string Output { get; set; } = string.Empty;
}
