using PowerArgs;

namespace Matrix.CliParsing.Benchmarks;

/// <summary>
/// The report form as a PowerArgs argument scaffold. PowerArgs derives the parser from the
/// type and binds each member by attribute; a value member that is not required is left at
/// its default when the option is absent.
/// </summary>
public class PowerArgsReportOptions
{
    [ArgRequired(PromptIfMissing = false)]
    [ArgShortcut("--input")]
    public string Input { get; set; } = string.Empty;

    [ArgShortcut("--format")]
    public string Format { get; set; } = string.Empty;

    [ArgShortcut("--verbose")]
    public bool Verbose { get; set; }

    [ArgPosition(0)]
    public string Output { get; set; } = string.Empty;
}

/// <summary>
/// The compact form as a PowerArgs argument scaffold, with an integer member so that value
/// conversion is part of the parse.
/// </summary>
public class PowerArgsCompactOptions
{
    [ArgRequired(PromptIfMissing = false)]
    [ArgShortcut("--level")]
    public int Level { get; set; }

    [ArgPosition(0)]
    [ArgRequired(PromptIfMissing = false)]
    public string Input { get; set; } = string.Empty;
}
