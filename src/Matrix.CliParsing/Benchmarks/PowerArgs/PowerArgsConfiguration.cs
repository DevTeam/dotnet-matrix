using PowerArgs;

namespace Matrix.CliParsing.Benchmarks;

/// <summary>
/// PowerArgs parses a typed scaffold directly, which is a parse-only call. A parse failure is
/// thrown as an <see cref="ArgException"/>, and the help text is generated from the argument
/// definition by the library's own template provider.
/// </summary>
internal static class PowerArgsConfiguration
{
    // The default template's Description+ wraps to Console.BufferWidth. PowerArgs 4.0.3
    // can loop indefinitely when CI has no terminal (width 0). Keep the library's
    // template and renderer, but let description columns grow without console wrapping.
    private static readonly string HelpTemplate = UsageTemplates.ConsoleTemplate.Replace("Description+", "Description");

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
        return new GeneratedHelp(ArgUsage.GenerateUsageFromTemplate(definition, HelpTemplate).ToString());
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
