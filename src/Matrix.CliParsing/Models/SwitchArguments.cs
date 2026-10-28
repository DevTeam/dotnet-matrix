namespace Matrix.CliParsing.Models;

/// <summary>
/// The state of a boolean switch together with the positional argument parsed from the same array.
/// </summary>
public sealed record SwitchArguments(bool Verbose, string Output)
{
    public static readonly SwitchArguments Expected = new(true, "out.txt");
    public static readonly SwitchArguments ExpectedOff = new(false, "out.txt");
}
