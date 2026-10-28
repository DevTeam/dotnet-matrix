using CliFx;
using CliFx.Binding;
using CliFx.Infrastructure;

namespace Matrix.CliParsing.Benchmarks;

// CliFx analyzer assumes all commands of the assembly are registered on one application,
// where two root commands would collide. Each form here is the root command of its own
// application, so the names never meet and the rule does not apply.
#pragma warning disable CliFx_CommandMustHaveUniqueName

/// <summary>
/// The report form as a CliFx command. CliFx 3 binds through a generated descriptor, so the
/// type must be partial and every bound member must be settable; a non-nullable reference
/// member or a non-optional value member is required by the parser.
/// </summary>
[Command]
public partial class CliFxReportCommand : ICommand
{
    [CommandOption("input")]
    public required string Input { get; set; }

    [CommandOption("format")]
    public string? Format { get; set; }

    [CommandOption("verbose")]
    public bool Verbose { get; set; }

    [CommandOption("tag")]
    public string[] Tags { get; set; } = [];

    [CommandParameter(0)]
    public required string Output { get; set; }

    public ValueTask ExecuteAsync(IConsole console)
    {
        CliFxConfiguration.ReportValues = new CliFxReportValues(
            Input,
            Format ?? string.Empty,
            Verbose,
            Tags,
            Output);
        return default;
    }
}

/// <summary>
/// The compact form as a CliFx command. It is the root command of its own application, which
/// is how two independent flat command definitions are declared without a command word.
/// </summary>
[Command]
public partial class CliFxCompactCommand : ICommand
{
    [CommandOption("level")]
    public required int Level { get; set; }

    [CommandParameter(0)]
    public required string Input { get; set; }

    public ValueTask ExecuteAsync(IConsole console)
    {
        CliFxConfiguration.CompactValues = new CliFxCompactValues(Level, Input);
        return default;
    }
}

#pragma warning restore CliFx_CommandMustHaveUniqueName
