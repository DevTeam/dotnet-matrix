using Spectre.Console.Cli;

namespace Matrix.CliParsing.Benchmarks;

/// <summary>
/// Holds one command instance, its writer and its application, all created when the
/// scenario object is built. The measured method only runs the parse.
/// </summary>
internal sealed class SpectreReportSetup
{
    public SpectreReportSetup()
    {
        Application = SpectreConfiguration.Create(Command, Writer);
    }

    public StringWriter Writer { get; } = new();

    public SpectreReportCommand Command { get; } = new();

    public CommandApp<SpectreReportCommand> Application { get; }
}

internal sealed class SpectreCompactSetup
{
    public SpectreCompactSetup()
    {
        Application = SpectreConfiguration.Create(Command, Writer);
    }

    public StringWriter Writer { get; } = new();

    public SpectreCompactCommand Command { get; } = new();

    public CommandApp<SpectreCompactCommand> Application { get; }
}
