using Cocona;
using Cocona.Builder;

namespace Matrix.CliParsing.Benchmarks;

/// <summary>
/// Cocona has no parse-only entry point: an application is built for one argument array
/// and runs the matched command. The reported scenarios measure one such run and read the
/// bound values from the captured delegate. A parse failure does not throw; Cocona writes
/// the diagnostic and sets an exit code, which is what a failure scenario reads.
/// </summary>
internal static class CoconaConfiguration
{
    public static void RunReport(CoconaCapture capture, string[] args)
    {
        capture.ReportValues = null;
        var application = CoconaApp.CreateBuilder(args).Build();
        application.AddCommand(capture.Report);
        application.Run();
    }

    public static void RunCompact(CoconaCapture capture, string[] args)
    {
        capture.CompactValues = null;
        var application = CoconaApp.CreateBuilder(args).Build();
        application.AddCommand(capture.Compact);
        application.Run();
    }

    public static ReportArguments ReadReport(CoconaCapture capture, string[] args)
    {
        RunReport(capture, args);
        var values = capture.ReportValues ?? new CoconaReportValues(string.Empty, string.Empty, false, [], string.Empty);
        return new ReportArguments(values.Input, values.Format, values.Output);
    }

    public static CompactArguments ReadCompact(CoconaCapture capture, string[] args)
    {
        RunCompact(capture, args);
        var values = capture.CompactValues ?? new CoconaCompactValues(0, string.Empty);
        return new CompactArguments(values.Level, values.Input);
    }

    public static TagArguments ReadTags(CoconaCapture capture, string[] args)
    {
        RunReport(capture, args);
        return new TagArguments([.. (capture.ReportValues?.Tags ?? [])]);
    }

    public static SwitchArguments ReadSwitch(CoconaCapture capture, string[] args)
    {
        RunReport(capture, args);
        var values = capture.ReportValues ?? new CoconaReportValues(string.Empty, string.Empty, false, [], string.Empty);
        return new SwitchArguments(values.Verbose, values.Output);
    }

    public static GeneratedHelp RenderHelp(StringWriter writer)
    {
        writer.GetStringBuilder().Clear();
        var output = Console.Out;
        try
        {
            Console.SetOut(writer);
            var application = CoconaApp.CreateBuilder(["--help"]).Build();
            application.AddCommand((string input, [Argument] string output) => { });
            application.Run();
        }
        finally
        {
            Console.SetOut(output);
        }

        return new GeneratedHelp(writer.ToString());
    }

    public static ParseOutcome ReadFailure(TextWriter writer, Action run)
    {
        Environment.ExitCode = 0;
        var error = Console.Error;
        try
        {
            Console.SetError(writer);
            run();
        }
        finally
        {
            Console.SetError(error);
        }

        var code = Environment.ExitCode;
        Environment.ExitCode = 0;
        return code == 0
            ? new ParseOutcome(true, string.Empty)
            : new ParseOutcome(false, writer.ToString().Trim());
    }
}
