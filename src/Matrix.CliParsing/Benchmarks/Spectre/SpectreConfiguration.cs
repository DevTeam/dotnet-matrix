using Spectre.Console;
using Spectre.Console.Cli;

namespace Matrix.CliParsing.Benchmarks;

/// <summary>
/// Spectre.Console.Cli has no parse-only entry point: one call parses the command line,
/// binds the settings object and executes the command. The reported scenarios measure
/// that call and read the bound settings from the instance the registrar supplies.
/// Strict parsing is enabled because the default setting silently ignores an option
/// the command does not declare, which would make the error scenario meaningless.
/// </summary>
internal static class SpectreConfiguration
{
    public static CommandApp<TCommand> Create<TCommand>(TCommand command, StringWriter writer)
        where TCommand : class, ICommand
    {
        var application = new CommandApp<TCommand>(new SpectreRegistrar(command));
        application.Configure(configuration =>
        {
            configuration.PropagateExceptions();
            configuration.Settings.ApplicationName = "tool";
            configuration.Settings.StrictParsing = true;
            configuration.Settings.Console = AnsiConsole.Create(new AnsiConsoleSettings
            {
                Out = new AnsiConsoleOutput(writer)
            });
        });
        return application;
    }

    public static ReportArguments ReadReport(SpectreReportCommand command, CommandApp<SpectreReportCommand> application, string[] args)
    {
        command.Captured = null;
        application.Run(args);
        var settings = command.Captured!;
        return new ReportArguments(settings.Input, settings.Format, settings.Output);
    }

    public static CompactArguments ReadCompact(SpectreCompactCommand command, CommandApp<SpectreCompactCommand> application, string[] args)
    {
        command.Captured = null;
        application.Run(args);
        var settings = command.Captured!;
        return new CompactArguments(settings.Level, settings.Input);
    }

    public static TagArguments ReadTags(SpectreReportCommand command, CommandApp<SpectreReportCommand> application, string[] args)
    {
        command.Captured = null;
        application.Run(args);
        return new TagArguments([.. command.Captured!.Tags]);
    }

    public static SwitchArguments ReadSwitch(SpectreReportCommand command, CommandApp<SpectreReportCommand> application, string[] args)
    {
        command.Captured = null;
        application.Run(args);
        var settings = command.Captured!;
        return new SwitchArguments(settings.Verbose, settings.Output);
    }

    public static GeneratedHelp RenderHelp(CommandApp<SpectreReportCommand> application, StringWriter writer)
    {
        writer.GetStringBuilder().Clear();
        application.Run(["--help"]);
        return new GeneratedHelp(writer.ToString());
    }

    public static ParseOutcome ReadFailure(Action run)
    {
        try
        {
            run();
            return new ParseOutcome(true, string.Empty);
        }
        catch (CommandRuntimeException exception)
        {
            return new ParseOutcome(false, exception.Message);
        }
    }
}
