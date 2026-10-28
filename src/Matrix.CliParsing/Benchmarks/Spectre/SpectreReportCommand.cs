using Spectre.Console.Cli;

namespace Matrix.CliParsing.Benchmarks;

/// <summary>
/// The command that receives the bound settings. Spectre.Console.Cli exposes no
/// parse-only entry point, so a reported scenario runs the command and reads what the
/// binder produced.
/// </summary>
internal sealed class SpectreReportCommand : Command<SpectreReportSettings>
{
    public SpectreReportSettings? Captured { get; set; }

    public override int Execute(CommandContext context, SpectreReportSettings settings, CancellationToken cancellationToken)
    {
        Captured = settings;
        return 0;
    }
}
