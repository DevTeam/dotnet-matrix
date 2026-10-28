using CommandLine;

namespace Matrix.CliParsing.Benchmarks;

internal static class CommandLineParserConfiguration
{
    private static readonly Parser Instance = new(settings => settings.HelpWriter = null);

    private static readonly StringWriter HelpWriter = new();

    private static readonly Parser HelpParser = new(settings => settings.HelpWriter = HelpWriter);

    public static ReportArguments ReadReport(string[] args) =>
        Instance.ParseArguments<CommandLineParserReportOptions>(args)
            .MapResult(
                report => new ReportArguments(report.Input, report.Format, report.Output),
                _ => new ReportArguments(string.Empty, string.Empty, string.Empty));

    public static CompactArguments ReadCompact(string[] args) =>
        Instance.ParseArguments<CommandLineParserCompactOptions>(args)
            .MapResult(
                compact => new CompactArguments(compact.Level, compact.Input),
                _ => new CompactArguments(0, string.Empty));

    public static TagArguments ReadTags(string[] args) =>
        Instance.ParseArguments<CommandLineParserReportOptions>(args)
            .MapResult(
                report => new TagArguments([.. report.Tags]),
                _ => new TagArguments([]));

    public static SwitchArguments ReadSwitch(string[] args) =>
        Instance.ParseArguments<CommandLineParserReportOptions>(args)
            .MapResult(
                report => new SwitchArguments(report.Verbose, report.Output),
                _ => new SwitchArguments(false, string.Empty));

    public static GeneratedHelp RenderHelp()
    {
        HelpWriter.GetStringBuilder().Clear();
        HelpParser.ParseArguments<CommandLineParserReportOptions>(["--help"]);
        return new GeneratedHelp(HelpWriter.ToString());
    }

    public static ParseOutcome ReadFailure(string[] args) =>
        ToOutcome(Instance.ParseArguments<CommandLineParserReportOptions>(args).Errors);

    public static ParseOutcome ReadCompactFailure(string[] args) =>
        ToOutcome(Instance.ParseArguments<CommandLineParserCompactOptions>(args).Errors);

    private static ParseOutcome ToOutcome(IEnumerable<Error> errors)
    {
        var list = errors.ToArray();
        return list.Length == 0
            ? new ParseOutcome(true, string.Empty)
            : new ParseOutcome(false, string.Join("; ", list.Select(error => error.Tag.ToString())));
    }
}
