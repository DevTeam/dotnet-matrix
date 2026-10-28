using Typin;

namespace Matrix.CliParsing.Benchmarks;

/// <summary>
/// Typin has no parse-only entry point: a built application parses one argument array and
/// executes the matched command. Each form is the root command of its own application, so
/// neither argument array carries a command word. A parse failure sets a non-zero exit code
/// and writes a diagnostic to the error stream, which is what a failure scenario reads.
/// </summary>
internal static class TypinConfiguration
{
    public static ReportArguments ReadReport(TypinMemoryConsole console, string[] args)
    {
        RunReport(console, args);
        var values = TypinReportCommand.Captured ?? new TypinReportValues(string.Empty, string.Empty, false, [], string.Empty);
        return new ReportArguments(values.Input, values.Format, values.Output);
    }

    public static CompactArguments ReadCompact(TypinMemoryConsole console, string[] args)
    {
        RunCompact(console, args);
        var values = TypinCompactCommand.Captured ?? new TypinCompactValues(0, string.Empty);
        return new CompactArguments(values.Level, values.Input);
    }

    public static TagArguments ReadTags(TypinMemoryConsole console, string[] args)
    {
        RunReport(console, args);
        return new TagArguments([.. (TypinReportCommand.Captured?.Tags ?? [])]);
    }

    public static SwitchArguments ReadSwitch(TypinMemoryConsole console, string[] args)
    {
        RunReport(console, args);
        var values = TypinReportCommand.Captured ?? new TypinReportValues(string.Empty, string.Empty, false, [], string.Empty);
        return new SwitchArguments(values.Verbose, values.Output);
    }

    public static GeneratedHelp RenderHelp(TypinMemoryConsole console)
    {
        console.ResetBuffers();
        RunReport(console, ["--help"]);
        return new GeneratedHelp(console.ReadOutput());
    }

    public static ParseOutcome ReadFailure(TypinMemoryConsole console, string[] args, bool compact)
    {
        console.ResetBuffers();
        var code = compact ? RunCompact(console, args) : RunReport(console, args);
        return code == 0
            ? new ParseOutcome(true, string.Empty)
            : new ParseOutcome(false, console.ReadError().Trim());
    }

    private static int RunReport(TypinMemoryConsole console, string[] args)
    {
        TypinReportCommand.Captured = null;
        var application = new CliApplicationBuilder()
            .UseConsole(console)
            .AddCommand(typeof(TypinReportCommand))
            .Build();
        return application.RunAsync(args).GetAwaiter().GetResult();
    }

    private static int RunCompact(TypinMemoryConsole console, string[] args)
    {
        TypinCompactCommand.Captured = null;
        var application = new CliApplicationBuilder()
            .UseConsole(console)
            .AddCommand(typeof(TypinCompactCommand))
            .Build();
        return application.RunAsync(args).GetAwaiter().GetResult();
    }
}
