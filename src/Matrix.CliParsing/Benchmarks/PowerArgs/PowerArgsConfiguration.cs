using PowerArgs;

namespace Matrix.CliParsing.Benchmarks;

/// <summary>
/// PowerArgs parses a typed scaffold directly, which is a parse-only call. A parse failure is
/// thrown as an <see cref="ArgException"/>, and the help text is generated from the argument
/// definition by the library's own template provider.
/// </summary>
internal static class PowerArgsConfiguration
{
    public static ReportArguments ReadReport(string[] args)
    {
        var options = Args.Parse<PowerArgsReportOptions>(args);
        return new ReportArguments(options.Input, options.Format, options.Output);
    }

    public static CompactArguments ReadCompact(string[] args)
    {
        var options = Args.Parse<PowerArgsCompactOptions>(args);
        return new CompactArguments(options.Level, options.Input);
    }

    public static SwitchArguments ReadSwitch(string[] args)
    {
        var options = Args.Parse<PowerArgsReportOptions>(args);
        return new SwitchArguments(options.Verbose, options.Output);
    }

    public static GeneratedHelp RenderHelp()
    {
        var definition = new CommandLineArgumentsDefinition(typeof(PowerArgsReportOptions));
        return new GeneratedHelp(ArgUsage.GenerateUsageFromTemplate(definition).ToString());
    }

    public static ParseOutcome ReadFailure(Func<object> run)
    {
        try
        {
            run();
            return new ParseOutcome(true, string.Empty);
        }
        catch (ArgException exception)
        {
            return new ParseOutcome(false, exception.Message);
        }
    }
}
