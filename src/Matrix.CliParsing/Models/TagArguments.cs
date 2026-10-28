namespace Matrix.CliParsing.Models;

/// <summary>
/// Repeated option values in the order they were supplied.
/// </summary>
public sealed record TagArguments(string[] Tags)
{
    public static readonly string[] Expected = ["a", "b", "c"];
}
