namespace Matrix.CliParsing;

/// <summary>
/// The fixed argument arrays every adapter parses. They are static so that setup
/// cost is identical for every library and nothing is allocated per iteration.
/// The positional value comes first, because a parser that binds every following
/// non-option token to the preceding option would otherwise swallow it.
/// </summary>
internal static class CliData
{
    public static readonly string[] OptionValue =
        ["out.txt", "--input", "in.txt", "--format", "json"];

    public static readonly string[] Positional =
        ["in.txt", "--level", "5"];

    public static readonly string[] RepeatedOption =
        ["out.txt", "--input", "in.txt", "--format", "json", "--tag", "a", "--tag", "b", "--tag", "c"];

    public static readonly string[] Switch =
        ["out.txt", "--input", "in.txt", "--format", "json", "--verbose"];

    public static readonly string[] WithoutSwitch =
        ["out.txt", "--input", "in.txt", "--format", "json"];

    public static readonly string[] Help = ["--help"];

    public static readonly string[] UnknownOption =
        ["out.txt", "--input", "in.txt", "--format", "json", "--bogus"];

    public static readonly string[] MissingRequiredOption =
        ["out.txt", "--format", "json"];

    public static readonly string[] InvalidType =
        ["in.txt", "--level", "many"];

    public static readonly string[] MissingRequiredArgument =
        ["--level", "5"];
}
