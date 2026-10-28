namespace Matrix.CliParsing.Models;

/// <summary>
/// Values of the option scenario: two options with values and one positional argument.
/// </summary>
public sealed record ReportArguments(string Input, string Format, string Output)
{
    public static readonly ReportArguments Expected = new("in.txt", "json", "out.txt");
}
