using Typin;
using Typin.Attributes;
using Typin.Console;

namespace Matrix.CliParsing.Benchmarks;

/// <summary>
/// The report form as a Typin command. Typin binds attributes to members and executes the
/// command, so the bound values are captured from the command instance.
/// </summary>
[Command]
public class TypinReportCommand : ICommand
{
    public static TypinReportValues? Captured { get; set; }

    [CommandOption("input", IsRequired = true)]
    public string Input { get; set; } = string.Empty;

    [CommandOption("format")]
    public string Format { get; set; } = string.Empty;

    [CommandOption("verbose")]
    public bool Verbose { get; set; }

    [CommandOption("tag")]
    public string[] Tags { get; set; } = [];

    [CommandParameter(0)]
    public string Output { get; set; } = string.Empty;

    public ValueTask ExecuteAsync(IConsole console)
    {
        Captured = new TypinReportValues(Input, Format, Verbose, Tags, Output);
        return default;
    }
}

/// <summary>
/// The compact form as a Typin command, with an integer member so that value conversion is
/// part of the parse. It is the root command of its own application, which is how two
/// independent flat command definitions are declared without a command word.
/// </summary>
[Command]
public class TypinCompactCommand : ICommand
{
    public static TypinCompactValues? Captured { get; set; }

    [CommandOption("level", IsRequired = true)]
    public int Level { get; set; }

    [CommandParameter(0)]
    public string Input { get; set; } = string.Empty;

    public ValueTask ExecuteAsync(IConsole console)
    {
        Captured = new TypinCompactValues(Level, Input);
        return default;
    }
}

public sealed record TypinReportValues(string Input, string Format, bool Verbose, string[] Tags, string Output);

public sealed record TypinCompactValues(int Level, string Input);
