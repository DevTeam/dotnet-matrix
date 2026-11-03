using CommandDotNet;

namespace Matrix.CliParsing.Benchmarks;

/// <summary>
/// The shared command model expressed with CommandDotNet. CommandDotNet is an application
/// framework rather than a parser: <see cref="AppRunner.Run"/> tokenizes the array, binds the
/// members of the command class it creates and then executes that method, and there is no
/// parse-only entry point. A reported scenario therefore makes that call and reads the values the
/// binder wrote. The runners and their settings are built once, because an application builds them
/// at startup, and a diagnostic is read from a writer installed for the duration of the call
/// because the library reports a failed parse to the console instead of returning it.
/// </summary>
internal static class CommandDotNetConfiguration
{
    private static readonly AppRunner ReportRunner = Create<CommandDotNetReportCommand>();

    private static readonly AppRunner CompactRunner = Create<CommandDotNetCompactCommand>();

    private static readonly AppRunner CompactModelRunner = Create<CommandDotNetCompactModelCommand>();

    public static ReportArguments ReadReport(string[] args)
    {
        CommandDotNetReportCommand.Captured.Reset();
        ReportRunner.Run(args);
        var captured = CommandDotNetReportCommand.Captured;
        return new ReportArguments(captured.Input, captured.Format, captured.Output);
    }

    public static CompactArguments ReadCompact(string[] args)
    {
        CommandDotNetCompactCommand.Captured.Reset();
        CompactRunner.Run(args);
        var captured = CommandDotNetCompactCommand.Captured;
        return new CompactArguments(captured.Level, captured.Input);
    }

    public static CompactArguments ReadCompactModel(string[] args)
    {
        CommandDotNetCompactModelCommand.Captured.Reset();
        CompactModelRunner.Run(args);
        var captured = CommandDotNetCompactModelCommand.Captured;
        return new CompactArguments(captured.Level, captured.Input);
    }

    public static TagArguments ReadTags(string[] args)
    {
        CommandDotNetReportCommand.Captured.Reset();
        ReportRunner.Run(args);
        var tags = CommandDotNetReportCommand.Captured.Tags;
        return new TagArguments(tags is null ? [] : [.. tags]);
    }

    public static SwitchArguments ReadSwitch(string[] args)
    {
        CommandDotNetReportCommand.Captured.Reset();
        ReportRunner.Run(args);
        var captured = CommandDotNetReportCommand.Captured;
        return new SwitchArguments(captured.Verbose, captured.Output);
    }

    public static GeneratedHelp RenderHelp()
    {
        var writer = new StringWriter();
        RunCaptured(ReportRunner, CliData.Help, writer);
        return new GeneratedHelp(writer.ToString());
    }

    public static ParseOutcome ReadReportFailure(string[] args) => ReadFailure(ReportRunner, args);

    public static ParseOutcome ReadCompactFailure(string[] args) => ReadFailure(CompactRunner, args);

    private static ParseOutcome ReadFailure(AppRunner runner, string[] args)
    {
        var writer = new StringWriter();
        var exitCode = RunCaptured(runner, args, writer);
        var diagnostic = writer.ToString().Trim();
        return exitCode == 0
            ? new ParseOutcome(true, diagnostic)
            : new ParseOutcome(
                false,
                diagnostic.Length > 0 ? diagnostic : $"CommandDotNet reported exit code {exitCode}.");
    }

    private static int RunCaptured(AppRunner runner, string[] args, StringWriter writer)
    {
        var previousOut = Console.Out;
        var previousError = Console.Error;
        try
        {
            Console.SetOut(writer);
            Console.SetError(writer);
            return runner.Run(args);
        }
        finally
        {
            Console.SetOut(previousOut);
            Console.SetError(previousError);
        }
    }

    private static AppRunner Create<T>()
        where T : class
    {
        var settings = new AppSettings();
        settings.Help.UsageAppName = "tool";
        return new AppRunner<T>(settings);
    }
}

/// <summary>
/// The command of the report form. CommandDotNet creates the instance itself, binds the parameters
/// of the default command and then executes it, so the bound values are read from
/// <see cref="Captured"/> afterwards.
/// </summary>
internal sealed class CommandDotNetReportCommand
{
    public static readonly CommandDotNetReportValues Captured = new();

    [DefaultCommand]
    public void Report(
        [Option("input")] string input,
        [Option("format")] string? format,
        [Option("verbose")] bool verbose,
        [Option("tag")] List<string>? tag,
        [Operand("output")] string output)
    {
        Captured.Input = input;
        Captured.Format = format ?? string.Empty;
        Captured.Verbose = verbose;
        Captured.Tags = tag;
        Captured.Output = output;
    }
}

internal sealed class CommandDotNetReportValues
{
    public string Input = string.Empty;

    public string Format = string.Empty;

    public bool Verbose;

    public List<string>? Tags;

    public string Output = string.Empty;

    public void Reset()
    {
        Input = string.Empty;
        Format = string.Empty;
        Verbose = false;
        Tags = null;
        Output = string.Empty;
    }
}

/// <summary>
/// The command of the compact form, with one option and one positional parameter.
/// </summary>
internal sealed class CommandDotNetCompactCommand
{
    public static readonly CommandDotNetCompactValues Captured = new();

    [DefaultCommand]
    public void Compact([Option("level")] int level, [Operand("input")] string input)
    {
        Captured.Level = level;
        Captured.Input = input;
    }
}

/// <summary>
/// The command of the compact form expressed as an argument model, which CommandDotNet binds into
/// a typed instance instead of into individual parameters.
/// </summary>
internal sealed class CommandDotNetCompactModelCommand
{
    public static readonly CommandDotNetCompactValues Captured = new();

    [DefaultCommand]
    public void Compact(CommandDotNetCompactModel model)
    {
        Captured.Level = model.Level;
        Captured.Input = model.Input;
    }
}

internal sealed class CommandDotNetCompactValues
{
    public int Level;

    public string Input = string.Empty;

    public void Reset()
    {
        Level = 0;
        Input = string.Empty;
    }
}

/// <summary>
/// A typed model of the compact form. The library treats a non-nullable operand without a default
/// value as required, so an absent positional argument is reported instead of left at a default.
/// </summary>
internal sealed class CommandDotNetCompactModel : IArgumentModel
{
    [Option("level")]
    public int Level { get; set; }

    [Operand("input")]
    public string Input { get; set; } = null!;
}
