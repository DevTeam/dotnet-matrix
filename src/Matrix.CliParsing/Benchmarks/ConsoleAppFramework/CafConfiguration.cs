using ConsoleAppFramework;

namespace Matrix.CliParsing.Benchmarks;

/// <summary>
/// ConsoleAppFramework generates the parser from the command signature. The generated
/// parser catches an argument failure itself, writes its message and sets
/// <see cref="Environment.ExitCode"/> to 1 instead of throwing, so a reported failure is
/// read from that exit code and the command words are read back from the handler.
/// The same builder holds the root command (the report form) and the compact command.
/// </summary>
internal static class CafConfiguration
{
    public static ConsoleApp.ConsoleAppBuilder Create(CafCapture capture)
    {
        var application = ConsoleApp.Create();
        application.Add("", capture.Report);
        application.Add("compact", capture.Compact);
        return application;
    }

    public static ReportArguments ReadReport(CafCapture capture, ConsoleApp.ConsoleAppBuilder application, string[] args)
    {
        capture.ReportValues = null;
        application.Run(args);
        var values = capture.ReportValues ?? new CafReportValues(string.Empty, string.Empty, false, [], string.Empty);
        return new ReportArguments(values.Input, values.Format, values.Output);
    }

    public static CompactArguments ReadCompact(CafCapture capture, ConsoleApp.ConsoleAppBuilder application, string[] args)
    {
        capture.CompactValues = null;
        application.Run(args);
        var values = capture.CompactValues ?? new CafCompactValues(0, string.Empty);
        return new CompactArguments(values.Level, values.Input);
    }

    public static TagArguments ReadTags(CafCapture capture, ConsoleApp.ConsoleAppBuilder application, string[] args)
    {
        capture.ReportValues = null;
        application.Run(args);
        return new TagArguments([.. capture.ReportValues!.Tags]);
    }

    public static SwitchArguments ReadSwitch(CafCapture capture, ConsoleApp.ConsoleAppBuilder application, string[] args)
    {
        capture.ReportValues = null;
        application.Run(args);
        var values = capture.ReportValues ?? new CafReportValues(string.Empty, string.Empty, false, [], string.Empty);
        return new SwitchArguments(values.Verbose, values.Output);
    }

    public static GeneratedHelp RenderHelp(StringWriter writer, ConsoleApp.ConsoleAppBuilder application)
    {
        writer.GetStringBuilder().Clear();
        var output = Console.Out;
        try
        {
            Console.SetOut(writer);
            application.Run(["--help"]);
        }
        finally
        {
            Console.SetOut(output);
        }

        return new GeneratedHelp(writer.ToString());
    }

    public static ParseOutcome ReadFailure(TextWriter writer, ConsoleApp.ConsoleAppBuilder application, string[] args)
    {
        Environment.ExitCode = 0;
        var output = Console.Out;
        try
        {
            Console.SetOut(writer);
            application.Run(args);
        }
        finally
        {
            Console.SetOut(output);
        }

        var code = Environment.ExitCode;
        Environment.ExitCode = 0;
        return code == 0
            ? new ParseOutcome(true, string.Empty)
            : new ParseOutcome(false, writer.ToString().Trim());
    }
}
