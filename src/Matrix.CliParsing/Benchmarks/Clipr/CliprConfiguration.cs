using clipr;
using clipr.Usage;

namespace Matrix.CliParsing.Benchmarks;

internal static class CliprConfiguration
{
    public static ReportArguments ReadReport(string[] args)
    {
        var options = CliParser.Parse<CliprOptions>(args);
        return new ReportArguments(options.Input, options.Format, options.Output);
    }

    public static CompactArguments ReadCompact(string[] args)
    {
        var options = CliParser.Parse<CliprCompactOptions>(args);
        return new CompactArguments(options.Level, options.Input);
    }

    public static TagArguments ReadTags(string[] args)
    {
        var options = CliParser.Parse<CliprOptions>(args);
        return new TagArguments([.. options.Tags]);
    }

    public static SwitchArguments ReadSwitch(string[] args)
    {
        var options = CliParser.Parse<CliprOptions>(args);
        return new SwitchArguments(options.Verbose, options.Output);
    }

    public static GeneratedHelp RenderHelp()
    {
        var parser = new CliParser<CliprOptions>(new CliprOptions());
        return new GeneratedHelp(new AutomaticHelpGenerator<CliprOptions>().GetHelp(parser.Config));
    }

    public static ParseOutcome ReadFailure(string[] args)
    {
        try
        {
            CliParser.Parse<CliprOptions>(args);
            return new ParseOutcome(true, string.Empty);
        }
        catch (ParseException exception)
        {
            return new ParseOutcome(false, exception.Message);
        }
    }

    public static ParseOutcome ReadCompactFailure(string[] args)
    {
        try
        {
            CliParser.Parse<CliprCompactOptions>(args);
            return new ParseOutcome(true, string.Empty);
        }
        catch (ParseException exception)
        {
            return new ParseOutcome(false, exception.Message);
        }
    }
}
