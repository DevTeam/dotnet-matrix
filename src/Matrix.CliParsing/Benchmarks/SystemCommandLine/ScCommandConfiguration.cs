using System.CommandLine;

namespace Matrix.CliParsing.Benchmarks;

/// <summary>
/// The shared command model expressed with System.CommandLine. The command tree is
/// built once, because an application defines it at startup.
/// </summary>
internal static class ScCommandConfiguration
{
    public static RootCommand Create()
    {
        var command = new RootCommand("tool")
        {
            new Option<string>("--input") { Required = true },
            new Option<string>("--format"),
            new Option<bool>("--verbose"),
            new Option<int>("--level"),
            new Option<string[]>("--tag"),
            new Argument<string>("output")
        };
        return command;
    }

    public static ReportArguments ReadReport(RootCommand command, string[] args)
    {
        var result = command.Parse(args);
        return new ReportArguments(
            result.GetValue<string>("--input") ?? string.Empty,
            result.GetValue<string>("--format") ?? string.Empty,
            result.GetValue<string>("output") ?? string.Empty);
    }

    public static CompactArguments ReadCompact(RootCommand command, string[] args)
    {
        var result = command.Parse(args);
        return new CompactArguments(
            result.GetValue<int>("--level"),
            result.GetValue<string>("output") ?? string.Empty);
    }

    public static TagArguments ReadTags(RootCommand command, string[] args)
    {
        var result = command.Parse(args);
        return new TagArguments(result.GetValue<string[]>("--tag") ?? []);
    }

    public static SwitchArguments ReadSwitch(RootCommand command, string[] args)
    {
        var result = command.Parse(args);
        return new SwitchArguments(
            result.GetValue<bool>("--verbose"),
            result.GetValue<string>("output") ?? string.Empty);
    }

    public static GeneratedHelp RenderHelp(RootCommand command)
    {
        var writer = new StringWriter();
        var previous = Console.Out;
        try
        {
            Console.SetOut(writer);
            command.Parse(["--help"]).Invoke();
        }
        finally
        {
            Console.SetOut(previous);
        }

        return new GeneratedHelp(writer.ToString());
    }

    public static ParseOutcome ReadFailure(RootCommand command, string[] args)
    {
        var result = command.Parse(args);
        return result.Errors.Count == 0
            ? new ParseOutcome(true, string.Empty)
            : new ParseOutcome(false, string.Join("; ", result.Errors.Select(error => error.Message)));
    }
}
