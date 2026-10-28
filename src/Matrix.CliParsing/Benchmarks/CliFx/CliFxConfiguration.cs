using CliFx;
using CliFx.Binding;
using CliFx.Infrastructure;

namespace Matrix.CliParsing.Benchmarks;

/// <summary>
/// CliFx binds a command class to a generated descriptor and runs it; there is no parse-only
/// entry point, so the measured call parses the line, binds the command and executes it. A
/// parse failure is reported as a non-zero exit code and a diagnostic on the error writer,
/// which is what a failure scenario reads. Each form is the root command of its own
/// application, so neither argument array carries a command word.
/// </summary>
internal static class CliFxConfiguration
{
    public static CliFxReportValues? ReportValues { get; set; }

    public static CliFxCompactValues? CompactValues { get; set; }

    public static ValueTask<int> RunReport(string[] args, IConsole? console = null)
    {
        ReportValues = null;
        return CreateReportApplication(console).RunAsync(args);
    }

    public static ValueTask<int> RunCompact(string[] args, IConsole? console = null)
    {
        CompactValues = null;
        return CreateCompactApplication(console).RunAsync(args);
    }

    public static ReportArguments ReadReport(string[] args)
    {
        RunReport(args).GetAwaiter().GetResult();
        var values = ReportValues ?? new CliFxReportValues(string.Empty, string.Empty, false, [], string.Empty);
        return new ReportArguments(values.Input, values.Format, values.Output);
    }

    public static CompactArguments ReadCompact(string[] args)
    {
        RunCompact(args).GetAwaiter().GetResult();
        var values = CompactValues ?? new CliFxCompactValues(0, string.Empty);
        return new CompactArguments(values.Level, values.Input);
    }

    public static TagArguments ReadTags(string[] args)
    {
        RunReport(args).GetAwaiter().GetResult();
        return new TagArguments([.. (ReportValues?.Tags ?? [])]);
    }

    public static SwitchArguments ReadSwitch(string[] args)
    {
        RunReport(args).GetAwaiter().GetResult();
        var values = ReportValues ?? new CliFxReportValues(string.Empty, string.Empty, false, [], string.Empty);
        return new SwitchArguments(values.Verbose, values.Output);
    }

    public static GeneratedHelp RenderHelp(FakeInMemoryConsole console)
    {
        CreateReportApplication(console).RunAsync(["--help"]).GetAwaiter().GetResult();
        return new GeneratedHelp(console.ReadOutputString());
    }

    public static ParseOutcome ReadFailure(string[] args, bool compact, FakeInMemoryConsole console)
    {
        var code = compact
            ? RunCompact(args, console).GetAwaiter().GetResult()
            : RunReport(args, console).GetAwaiter().GetResult();
        return code == 0
            ? new ParseOutcome(true, string.Empty)
            : new ParseOutcome(false, console.ReadErrorString().Trim());
    }

    private static CommandLineApplication CreateReportApplication(IConsole? console) =>
        CreateApplication(CliFxReportCommand.Descriptor, console);

    private static CommandLineApplication CreateCompactApplication(IConsole? console) =>
        CreateApplication(CliFxCompactCommand.Descriptor, console);

    private static CommandLineApplication CreateApplication(CommandDescriptor descriptor, IConsole? console)
    {
        var builder = new CommandLineApplicationBuilder().AddCommand(descriptor);
        if (console is not null)
        {
            builder = builder.UseConsole(console);
        }

        return builder.Build();
    }
}

internal sealed record CliFxReportValues(string Input, string Format, bool Verbose, string[] Tags, string Output);

internal sealed record CliFxCompactValues(int Level, string Input);
