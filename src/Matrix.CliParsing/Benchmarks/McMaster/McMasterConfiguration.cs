using System.ComponentModel.DataAnnotations;
using McMaster.Extensions.CommandLineUtils;

namespace Matrix.CliParsing.Benchmarks;

/// <summary>
/// McMaster binds a typed model but validates required members separately, through
/// <see cref="CommandLineApplication.GetValidationResult"/>.
/// </summary>
internal static class McMasterConfiguration
{
    public static CommandLineApplication<TModel> Create<TModel>()
        where TModel : class, new()
    {
        var application = new CommandLineApplication<TModel>();
        application.Conventions.UseDefaultConventions();
        // A parsing failure prints a usage hint through the application's own writers.
        application.Out = TextWriter.Null;
        application.Error = TextWriter.Null;
        // A single application instance is reused for every parse, so each parse must
        // start from a fresh model instead of the values bound by the previous one.
        application.ModelFactory = () => new TModel();
        return application;
    }

    public static ReportArguments ReadReport(CommandLineApplication<McMasterReportOptions> application, string[] args)
    {
        application.Parse(args);
        Reject(application);
        var model = application.Model;
        return new ReportArguments(model.Input, model.Format, model.Output);
    }

    public static CompactArguments ReadCompact(CommandLineApplication<McMasterCompactOptions> application, string[] args)
    {
        application.Parse(args);
        Reject(application);
        var model = application.Model;
        return new CompactArguments(model.Level, model.Input);
    }

    public static TagArguments ReadTags(CommandLineApplication<McMasterReportOptions> application, string[] args)
    {
        application.Parse(args);
        Reject(application);
        return new TagArguments([.. application.Model.Tags]);
    }

    public static SwitchArguments ReadSwitch(CommandLineApplication<McMasterReportOptions> application, string[] args)
    {
        application.Parse(args);
        Reject(application);
        var model = application.Model;
        return new SwitchArguments(model.Verbose, model.Output);
    }

    public static GeneratedHelp RenderHelp(CommandLineApplication<McMasterReportOptions> application) =>
        new(application.GetHelpText());

    public static ParseOutcome ReadFailure(CommandLineApplication<McMasterReportOptions> application, string[] args) =>
        Fail(() =>
        {
            application.Parse(args);
            Reject(application);
        });

    public static ParseOutcome ReadCompactFailure(CommandLineApplication<McMasterCompactOptions> application, string[] args) =>
        Fail(() =>
        {
            application.Parse(args);
            Reject(application);
        });

    private static void Reject(CommandLineApplication application)
    {
        var result = application.GetValidationResult();
        if (result != ValidationResult.Success)
        {
            throw new CommandParsingException(application, result?.ErrorMessage ?? "Invalid command line.");
        }
    }

    private static ParseOutcome Fail(Action parse)
    {
        try
        {
            parse();
            return new ParseOutcome(true, string.Empty);
        }
        catch (CommandParsingException exception)
        {
            return new ParseOutcome(false, exception.Message);
        }
    }
}
