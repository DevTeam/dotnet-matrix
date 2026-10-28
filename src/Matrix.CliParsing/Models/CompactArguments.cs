namespace Matrix.CliParsing.Models;

/// <summary>
/// A typed model with an integer option and a positional argument.
/// </summary>
public sealed record CompactArguments(int Level, string Input)
{
    public static readonly CompactArguments Expected = new(5, "in.txt");
}
